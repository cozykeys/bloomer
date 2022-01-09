import enum
import math
import os
import pcbnew
import sys
import wx
import traceback

POSITION_SCALE = 1000000.0
ROTATION_SCALE = 10

MID_X = 200
MID_Y = 100

def bloomer_repo_dir():
    # /path/to/bloomer/scripts/kicad_plugin/bloomer_kicad_plugin_action.py
    script_path = os.path.realpath(__file__)

    # /path/to/bloomer/scripts/kicad_plugin
    kicad_plugin_dir = os.path.dirname(script_path)

    # /path/to/bloomer/scripts
    scripts_dir = os.path.dirname(kicad_plugin_dir)

    # /path/to/bloomer
    return os.path.dirname(scripts_dir)


def ref_to_index(ref):
    return int(ref.lower().replace("d", "").replace("k", ""))


class LogLevel(enum.IntEnum):
    ERROR = 0
    WARN = 1
    INFO = 2
    DEBUG = 3
    TRACE = 4

class BloomerKicadPluginDialog(wx.Dialog):
    def __init__(self, parent):
        wx.Dialog.__init__(self, parent, title="Bloomer Dialog", style=wx.RESIZE_BORDER)

        self.panel = wx.Panel(self)

        title = wx.StaticText(self.panel, wx.ID_ANY, "Bloomer Plugin Log")
        self.log = wx.TextCtrl(
            self.panel, style=wx.TE_READONLY | wx.TE_MULTILINE | wx.TE_RICH
        )
        button = wx.Button(self.panel, wx.ID_OK, label="OK")

        topSizer = wx.BoxSizer(wx.VERTICAL)
        titleSizer = wx.BoxSizer(wx.HORIZONTAL)
        logSizer = wx.BoxSizer(wx.HORIZONTAL)
        buttonSizer = wx.BoxSizer(wx.HORIZONTAL)

        titleSizer.Add(title, 0, wx.ALL, 5)
        logSizer.Add(self.log, 1, wx.ALL | wx.EXPAND, 5)
        buttonSizer.Add(button, 0, wx.ALL | wx.EXPAND, 5)

        topSizer.Add(titleSizer, 0, wx.CENTER)
        topSizer.Add(logSizer, 1, wx.ALL | wx.EXPAND, 5)
        topSizer.Add(buttonSizer, 0, wx.ALL | wx.CENTER, 5)

        self.panel.SetSizerAndFit(topSizer)

    def LogClear(self, text):
        self.log.Clear()

    def LogAppend(self, level, text):
        lookup = {
            LogLevel.ERROR: ("ERROR", wx.RED),
            LogLevel.WARN: ("WARN", wx.YELLOW),
            LogLevel.INFO: ("INFO", wx.BLUE),
            LogLevel.DEBUG: ("DEBUG", wx.GREEN),
            LogLevel.TRACE: ("TRACE", wx.LIGHT_GREY),
        }

        (label, color) = lookup[level]

        self.log.SetDefaultStyle(wx.TextAttr(color))
        self.log.AppendText("[{}] {}\n".format(label, text))
        self.log.SetDefaultStyle(wx.TextAttr(wx.BLACK))


class BloomerKicadPluginAction(pcbnew.ActionPlugin):
    def defaults(self):
        """
        defaults is a member of pcbnew.ActionPlugin and should be overriden.
        """
        self.name = "Bloomer Plugin"
        self.category = "Modify PCB"
        self.description = "Place Bloomer components"
        self.show_toolbar_button = True
        self.icon_file_name = os.path.join(
            bloomer_repo_dir(), "icon", "bloomer_icon_32x32.png"
        )
        self.version = "0.0.9"
        self.wx_version = wx.version()

    def Run(self):
        """
        Run is a member of pcbnew.ActionPlugin and should be overriden.
        """
        try:
            self._run()
        except Exception:
            with open('/home/pewing/kicad_plugin_failure.txt', 'a') as f:
                exception_str = traceback.format_exc()
                f.write('{}\n'.format(exception_str))
                self._log_error('{}'.format(exception_str))

    def _run(self):
        sys.path.append(os.path.join(bloomer_repo_dir(), 'scripts'))
        from lib import SwitchData, Logger, Corner

        self.dialog = None
        self.footprints = None
        self.log_level = LogLevel.INFO

        # local_build indicates whether or not a locally built version of KiCad
        # is being used. There are some differences between HEAD and the most
        # recently released version that need to be accounted for.
        self.local_build = False

        self.logger = Logger()
        self._initialize_dialog()
        self._log_info("Running Bloomer KiCad plugin version {}".format(self.version))
        self._log_info("Running wxPython version {}".format(self.wx_version))
        self._log_info("Logging to directory {}".format(self.logger.path))

        if self.local_build:
            self._initialize_footprint_cache()
        pcb = pcbnew.GetBoard()

        self._log_info("Loading switch data")
        self.switch_data = SwitchData()

        # Set switch and diode positions
        key_count = 87
        for i in range(0, key_count):
            key_id = "K{}".format(str(i).zfill(2))
            key = pcb.FindModuleByReference(key_id)
            if key == None:
                raise Exception("No key with id {} found".format(key_id))
            self._set_key_position(key)

            diode_id = "D{}".format(str(i).zfill(2))
            diode = pcb.FindModuleByReference(diode_id)
            if diode == None:
                raise Exception("No diode with id {} found".format(diode_id))
            self._set_diode_position(diode)

        capacitor_count = 20
        resistor_count = 6
        led_count = 12
        hole_count = 9

        capacitors = ['C{}'.format(i) for i in range(1, capacitor_count+1)]
        resistors = ['R{}'.format(i) for i in range(1, resistor_count+1)]
        leds = ['LED{}'.format(i) for i in range(1, led_count+1)]
        holes = ['H{}'.format(i) for i in range(1, hole_count+1)]
        other = ["SW1", "MCU1", "Y1", "USB1"]

        components = capacitors + resistors + leds + holes + other

        for component_id in components:
            component = pcb.FindModuleByReference(component_id)
            if component == None:
                raise Exception("No component with id {} found".format(component_id))
            self._set_footprint_position(component)

        # Remove all existing drawings on the Edge.Cuts line, then add the
        # desired edge cut segments
        for d in pcb.GetDrawings():
            if d.GetLayerName() == "Edge.Cuts":
                self._log_info("Found a drawing on Edge.Cuts layer")
                pcb.Remove(d)
        self._draw_edge_cuts(pcb)

    def _log_error(self, message):
        self._log(LogLevel.ERROR, message)

    def _log_warn(self, message):
        self._log(LogLevel.WARN, message)

    def _log_info(self, message):
        self._log(LogLevel.INFO, message)

    def _log_debug(self, message):
        self._log(LogLevel.DEBUG, message)

    def _log_trace(self, message):
        self._log(LogLevel.TRACE, message)

    def _log(self, level, message):
        if int(level) > int(self.log_level):
            return
        if self.logger != None:
            self.logger.log(message)
        if self.dialog != None:
            self.dialog.LogAppend(level, message)

    def _initialize_dialog(self):
        pcbnew_window = None
        for w in wx.GetTopLevelWindows():
            if w.GetTitle().startswith("Pcbnew"):
                pcbnew_window = w
                self._log_info("Pcbnew window found!")
                break

        self.dialog = None
        for child in w.GetChildren():
            if type(child) == BloomerKicadPluginDialog:
                self.dialog = child
                self._log_info("Bloomer window found!")
                break

        if self.dialog == None:
            self.dialog = BloomerKicadPluginDialog(pcbnew_window)

        self.dialog.Show()
        self.dialog.SetSize(800, 600)

    def _initialize_footprint_cache(self):
        self.footprints = {}
        libraryNames = pcbnew.GetFootprintLibraries()
        self._log_info("wtf3")
        self._log_info("Initializing footprint cache:")
        self.log_debug("Footprints:")
        count = 0
        for l in libraryNames:
            self.log_debug("    {}".format(l))
            if not l in self.footprints:
                self.footprints[l] = []
            for f in pcbnew.GetFootprints(l):
                self.log_debug("        {}".format(f))
                self.footprints[l].append(f)
                count += 1
        self._log_info("Footprint cache initialized; {} footprints found".format(count))

    def _set_key_position(self, key):
        ref = key.GetReference()
        pos = key.GetPosition()
        self._log_info("Setting position of key {}".format(ref))
        s = self.switch_data.get_switch_by_index(ref_to_index(ref))
        pos.x = int(s["x"] * POSITION_SCALE)
        pos.y = int(s["y"] * POSITION_SCALE)
        key.SetPosition(pos)
        key.SetOrientation(-s["rotation"] * ROTATION_SCALE)

    def _set_diode_position(self, diode):
        ref = diode.GetReference()
        pos = diode.GetPosition()
        self._log_info("Setting position of diode {}".format(ref))
        s = self.switch_data.get_switch_by_index(ref_to_index(ref))
        rot = None
        theta = None

        # Distance from the center of the switch footprint to the center of the
        # diode footprint
        d = 8.2625
        if s["diode_position"] == "left":
            theta = math.pi + math.radians(s["rotation"])
            rot = 270 - s["rotation"]
        elif s["diode_position"] == "right":
            theta = 0 + math.radians(s["rotation"])
            rot = 270 - s["rotation"]
        elif s["diode_position"] == "top":
            theta = (0.5 * math.pi) + math.radians(s["rotation"])
            rot = 180 - s["rotation"]
        elif s["diode_position"] == "bottom":
            theta = (1.5 * math.pi) + math.radians(s["rotation"])
            rot = 180 - s["rotation"]
        else:
            raise Exception("Unsupported diode position {}".format(s["diode_position"]))

        pos.x = int((s["x"] + d * math.cos(theta)) * POSITION_SCALE)
        pos.y = int((s["y"] + d * math.sin(theta)) * POSITION_SCALE)
        diode.SetPosition(pos)
        if diode.GetLayerName() != "B.Cu":
            self._log_info(
                "Flipping diode {} because layer name {} is not B.Cu".format(
                    ref, diode.GetLayerName()
                )
            )
            diode.Flip(pos)
        diode.SetOrientation(rot * ROTATION_SCALE)

    def _set_footprint_position(self, footprint):
        lookup_table = {
            # Capacitors
            "C1":  ( 168.275,  126.365,  90.0 ),
            "C2":  ( 178.435,  126.365,  90.0 ),
            "C3":  ( 164.465,  136.525,  90.0 ),
            "C4":  ( 168.275,  147.955, 270.0 ),
            "C5":  ( 183.515,  133.35,   90.0 ),
            "C6":  ( 179.07,   147.955, 270.0 ),
            "C7":  ( 163.195,  141.605, 180.0 ),
            "C8":  ( 161.925,  136.525, 270.0 ),

            # LED Capacitors
"C9": (255.063, 68.83, 280.0),
"C10": (290.674, 51.381, 280.0),
"C11": (329.584, 52.644, 280.0),
"C12": (332.988, 129.419, 100.0),
"C13": (294.078, 128.156, 100.0),
"C14": (258.467, 145.605, 100.0),
"C15": (131.705, 143.872, 80.0),
"C16": (96.094, 126.423, 80.0),
"C17": (57.184, 127.686, 80.0),
"C18": (80.244, 54.377, 260.0),
"C19": (119.154, 53.114, 260.0),
"C20": (154.765, 70.563, 260.0),

            # LEDs
"LED1": (250.149, 69.696, 190),
"LED2": (285.76, 52.247, 190),
"LED3": (324.67, 53.51, 190),
"LED4": (337.902, 128.552, 10),
"LED5": (298.992, 127.289, 10),
"LED6": (263.381, 144.738, 10),
"LED7": (136.619, 144.738, 350),
"LED8": (101.008, 127.289, 350),
"LED9": (62.098, 128.552, 350),
"LED10": (75.33, 53.51, 170),
"LED11": (114.24, 52.247, 170),
"LED12": (149.851, 69.696, 170),

            # Resistors
            "R1":  (  164.465,  130.175, 90.0 ),
            "R2":  (  182.245,  147.955, 90.0 ),
            "R3":  (  201.27,    52.07,  90.0 ),
            "R4":  (  198.73,    52.07,  90.0 ),
            "R5":  (  196.19,    52.07,  90.0 ),
            "R6":  (  203.81,    52.07,  90.0 ),

            # Spacer Holes
            "H1": ( 258.661,  63.119, 0 ),
            "H2": ( 334.051,  51.856, 0 ),
            "H3": ( 347.283, 126.898, 0 ),
            "H4": ( 271.893, 138.161, 0 ),
            "H5": ( 128.107, 138.161, 0 ),
            "H6": (  52.717, 126.898, 0 ),
            "H7": (  65.949,  51.856, 0 ),
            "H8": ( 141.339,  63.119, 0 ),
            "H9": ( 200.0,   126.996, 0 ),

            # Other Components
            "SW1":   ( MID_X,    78.4975,  0.0 ),
            "MCU1":  ( 173.99,  138.43,    0.0 ),
            "Y1":    ( 173.355,  126.365, 45.0 ),
            "USB1":  ( MID_X,    40.0,     0.0 ),

        }

        ref = footprint.GetReference()
        pos = footprint.GetPosition()
        (x, y, r) = lookup_table[ref]
        pos.x = int(x * POSITION_SCALE)
        pos.y = int(y * POSITION_SCALE)
        footprint.SetPosition(pos)
        footprint.SetOrientation(r * ROTATION_SCALE)
        if footprint.GetLayerName() != "B.Cu":
            self._log_info(
                "Flipping footprint {} because layer name {} is not B.Cu".format(
                    ref, footprint.GetLayerName()
                )
            )
            footprint.Flip(pos)

    def _draw_edge_cuts(self, pcb):
        vertices = [
            (  50.497,  29.788 ),
            ( 108.168,  31.833 ),
            ( 126.928,  35.141 ),
            ( 273.072,  35.141 ),
            ( 291.832,  31.833 ),
            ( 349.503,  29.788 ),
            ( 369.351, 142.35  ),
            ( 331.83,  148.966 ),
            ( 276.069, 161.845 ),
            ( 238.548, 168.461 ),
            ( 228.575, 170.296 ),
            ( 171.425, 170.296 ),
            ( 161.452, 168.461 ),
            ( 123.931, 161.845 ),
            (  68.17,  148.966 ),
            (  30.649, 142.35  ),
        ]

        vertices = [
            (50.497,29.788),
            (108.168,31.833),
            (126.928,35.141),
            (163.408,47.666),
            (194.727,47.666),
            (194.727,41.801),
            (195.362,41.166),
            (204.638,41.166),
            (205.273,41.801),
            (205.273,47.666),
            (236.592,47.666),
            (273.072,35.141),
            (291.832,31.833),
            (349.503,29.788),
            (369.351,142.35),
            (331.83,148.966),
            (276.069,161.845),
            (238.548,168.461),
            (228.575,170.296),
            (171.425,170.296),
            (161.452,168.461),
            (123.931,161.845),
            (68.17,148.966),
            (30.649,142.35),
        ]

        l = len(vertices)
        for i in range(0, l):
            start = vertices[i]
            end = vertices[(i + 1) % l]
            segment = pcbnew.DRAWSEGMENT()
            segment.SetStartX(int(start[0] * POSITION_SCALE))
            segment.SetStartY(int(start[1] * POSITION_SCALE))
            segment.SetEndX(int(end[0] * POSITION_SCALE))
            segment.SetEndY(int(end[1] * POSITION_SCALE))
            segment.SetAngle(int(90 * ROTATION_SCALE))
            segment.SetWidth(int(0.3 * POSITION_SCALE))
            segment.SetLayer(pcbnew.Edge_Cuts)
            pcb.Add(segment)

BloomerKicadPluginAction().register()
