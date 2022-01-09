#!/usr/bin/env python3

import json
import os
import argparse
import math
from typing import List, Dict, Tuple
from lib import (
    SvgStyle,
    SvgWriter,
    Vector2D,
    Segment2D,
    Polygon2D,
    get_bloomer_repo_dir,
    SwitchData,
    Corner,
)

# To keep traces 0.635 mm apart when moving them at a 45* angle this is the
# extra distance required per track in the X dimension.
ANGLE_DELTA = math.tan(math.radians(45.0 / 2.0)) * 0.635

net_ids = {
    '""': 0,
    '"Net-(C1-Pad1)"': 1,
    'GND': 2,
    '"Net-(C2-Pad1)"': 3,
    'VCC': 4,
    '/usb_cap': 5,
    '"Net-(D01-Pad2)"': 6,
    '/row0': 7,
    '"Net-(D02-Pad2)"': 8,
    '"Net-(D03-Pad2)"': 9,
    '"Net-(D04-Pad2)"': 10,
    '"Net-(D05-Pad2)"': 11,
    '"Net-(D06-Pad2)"': 12,
    '"Net-(D07-Pad2)"': 13,
    '"Net-(D08-Pad2)"': 14,
    '"Net-(D09-Pad2)"': 15,
    '"Net-(D10-Pad2)"': 16,
    '"Net-(D11-Pad2)"': 17,
    '"Net-(D12-Pad2)"': 18,
    '"Net-(D13-Pad2)"': 19,
    '"Net-(D14-Pad2)"': 20,
    '"Net-(D15-Pad2)"': 21,
    '"Net-(D16-Pad2)"': 22,
    '/row1': 23,
    '"Net-(D17-Pad2)"': 24,
    '"Net-(D18-Pad2)"': 25,
    '"Net-(D19-Pad2)"': 26,
    '"Net-(D20-Pad2)"': 27,
    '"Net-(D21-Pad2)"': 28,
    '"Net-(D22-Pad2)"': 29,
    '"Net-(D23-Pad2)"': 30,
    '"Net-(D24-Pad2)"': 31,
    '"Net-(D25-Pad2)"': 32,
    '"Net-(D26-Pad2)"': 33,
    '"Net-(D27-Pad2)"': 34,
    '"Net-(D28-Pad2)"': 35,
    '"Net-(D29-Pad2)"': 36,
    '"Net-(D30-Pad2)"': 37,
    '"Net-(D31-Pad2)"': 38,
    '/row2': 39,
    '"Net-(D32-Pad2)"': 40,
    '"Net-(D33-Pad2)"': 41,
    '"Net-(D34-Pad2)"': 42,
    '"Net-(D35-Pad2)"': 43,
    '"Net-(D36-Pad2)"': 44,
    '"Net-(D37-Pad2)"': 45,
    '"Net-(D38-Pad2)"': 46,
    '"Net-(D39-Pad2)"': 47,
    '"Net-(D40-Pad2)"': 48,
    '"Net-(D41-Pad2)"': 49,
    '"Net-(D42-Pad2)"': 50,
    '"Net-(D43-Pad2)"': 51,
    '"Net-(D44-Pad2)"': 52,
    '"Net-(D45-Pad2)"': 53,
    '/row3': 54,
    '"Net-(D46-Pad2)"': 55,
    '"Net-(D47-Pad2)"': 56,
    '"Net-(D48-Pad2)"': 57,
    '"Net-(D49-Pad2)"': 58,
    '"Net-(D50-Pad2)"': 59,
    '"Net-(D51-Pad2)"': 60,
    '"Net-(D52-Pad2)"': 61,
    '"Net-(D53-Pad2)"': 62,
    '"Net-(D54-Pad2)"': 63,
    '"Net-(D55-Pad2)"': 64,
    '"Net-(D56-Pad2)"': 65,
    '"Net-(D57-Pad2)"': 66,
    '"Net-(D58-Pad2)"': 67,
    '"Net-(D59-Pad2)"': 68,
    '/row4': 69,
    '"Net-(D60-Pad2)"': 70,
    '"Net-(D61-Pad2)"': 71,
    '"Net-(D62-Pad2)"': 72,
    '"Net-(D63-Pad2)"': 73,
    '"Net-(D64-Pad2)"': 74,
    '"Net-(D65-Pad2)"': 75,
    '"Net-(D66-Pad2)"': 76,
    '"Net-(D67-Pad2)"': 77,
    '"Net-(D68-Pad2)"': 78,
    '"Net-(D69-Pad2)"': 79,
    '"Net-(D70-Pad2)"': 80,
    '"Net-(D71-Pad2)"': 81,
    '"Net-(D72-Pad2)"': 82,
    '"Net-(D73-Pad2)"': 83,
    '"Net-(D74-Pad2)"': 84,
    '/row5': 85,
    '"Net-(D75-Pad2)"': 86,
    '"Net-(D76-Pad2)"': 87,
    '"Net-(D77-Pad2)"': 88,
    '"Net-(D78-Pad2)"': 89,
    '"Net-(D79-Pad2)"': 90,
    '"Net-(D80-Pad2)"': 91,
    '"Net-(D81-Pad2)"': 92,
    '"Net-(D82-Pad2)"': 93,
    '"Net-(D83-Pad2)"': 94,
    '"Net-(D84-Pad2)"': 95,
    '"Net-(D85-Pad2)"': 96,
    '"Net-(D86-Pad2)"': 97,
    '/col0': 98,
    '/col1': 99,
    '/col2': 100,
    '/col3': 101,
    '/col4': 102,
    '/col5': 103,
    '/col6': 104,
    '/col7': 105,
    '/col8': 106,
    '/col9': 107,
    '/col10': 108,
    '/col11': 109,
    '/col12': 110,
    '/col13': 111,
    '/col14': 112,
    '/rgb': 113,
    '/usb_d-': 114,
    '/usb_d+': 115,
    '"Net-(D00-Pad2)"': 116,
    '"Net-(R3-Pad2)"': 117,
    '"Net-(R4-Pad2)"': 118,
    '"Net-(R5-Pad2)"': 119,
    '"Net-(R6-Pad2)"': 120,
    '"Net-(USB1-Pad9)"': 121,
    '"Net-(USB1-Pad3)"': 122,
    '"Net-(LED1-Pad2)"': 123,
    '"Net-(LED2-Pad2)"': 124,
    '"Net-(LED3-Pad2)"': 125,
    '"Net-(LED4-Pad2)"': 126,
    '"Net-(LED5-Pad2)"': 127,
    '"Net-(LED6-Pad2)"': 128,
    '"Net-(LED7-Pad2)"': 129,
    '"Net-(LED8-Pad2)"': 130,
    '"Net-(LED10-Pad4)"': 131,
    '"Net-(LED10-Pad2)"': 132,
    '"Net-(LED11-Pad2)"': 133,
    '"Net-(LED12-Pad2)"': 134,
    '"Net-(MCU1-Pad13)"': 135,
    '"Net-(MCU1-Pad32)"': 136,
    '"Net-(MCU1-Pad33)"': 137,
    '"Net-(MCU1-Pad42)"': 138,
    '"Net-(MCU1-Pad18)"': 139,
    '"Net-(MCU1-Pad19)"': 140,
}

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def project(self, distance, theta):
        return Point(
            self.x + distance * math.cos(theta),
            self.y + distance * math.sin(theta))

    def to_vec2d(self):
        return Vector2D(self.x, self.y)

class Path:
    def __init__(self, points):
        self.points = points

    def print_segments(self, layer, net_id):
        for i in range(1, len(self.points)):
            p1 = self.points[i]
            p2 = self.points[i-1]
            print(format_segment(p1, p2, layer, net_id))

class Segment:
    def __init__(self, start, end):
        self.start = start
        self.end = end

    def dx(self):
        return self.end.x - self.start.x

    def dy(self):
        return self.end.y - self.start.y

    def slope(self):
        return math.nan if self.dx() == 0 else self.dy() / self.dx()

    def to_line(self):
        slope = self.slope();
        y_intercept = math.nan if math.isnan(slope) else self.start.y - (slope * self.start.x)
        return Line(slope, y_intercept);

class Line:
    def __init__(self, slope, y_intercept):
        self.slope = slope
        self.y_intercept = y_intercept

    def intersection(self, other):
        if self.slope == other.slope:
            return None
        
        x = (other.y_intercept - self.y_intercept) / (self.slope - other.slope)
        y = self.slope * x + self.y_intercept;

        return Point(x, y);


def intersection_old(p1, theta1, p2, theta2):
    l1 = Segment(p1, p1.project(1.0, theta1)).to_line()
    l2 = Segment(p2, p2.project(1.0, theta2)).to_line()
    return l1.intersection(l2)


def intersection(p1, theta1, p2, theta2):
    s1 = Segment2D(p1.to_vec2d(), p1.project(1.0, theta1).to_vec2d())
    s2 = Segment2D(p2.to_vec2d(), p2.project(1.0, theta2).to_vec2d())

    v = s1.intersection(s2)
    return Point(v.x, v.y)


def build_path(start, movements):
    '''
    l should be a list of:
        [
            (magnitude, theta),
        ]
    '''
    points = [ start ]
    for m in movements:
        prev = points[len(points)-1]
        d = m[0]
        theta = m[1]
        points.append(
            Point(
                prev.x + d * math.cos(theta),
                prev.y + d * math.sin(theta)))
    return Path(points)


def build_path_2(start, movements):
    '''
    movements should be a list of:
        [
            [
                (magnitude, theta),
                (magnitude, theta),
            ],
            [
                (magnitude, theta),
                (magnitude, theta),
            ],
        ]
    '''
    points = [ start ]
    for m_array in movements:
        prev = points[len(points)-1]
        curr = Point(prev.x, prev.y)
        for m in m_array:
            d = m[0]
            theta = m[1]
            curr = Point(
                curr.x + d * math.cos(theta),
                curr.y + d * math.sin(theta))
        points.append(curr)
    return Path(points)


def add_generate_traces_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "gen-traces", help="Generate traces for PCB"
    )
    parser.set_defaults(func=generate_traces)


def format_via(x, y, net_id):
  at_str     = '(at {} {})'.format(x, y)
  size_str   = '(size 0.8)'
  drill_str  = '(drill 0.4)'
  layers_str = '(layers F.Cu B.Cu)'
  net_str    = '(net {})'.format(net_id)
  return '(via {} {} {} {} {})'.format(at_str, size_str, drill_str, layers_str, net_str)


def format_segment(start, end, layer, net_id):
    start = '(start {} {})'.format(round(start.x, 5), round(start.y, 5))
    end = '(end {} {})'.format(round(end.x, 5), round(end.y, 5))
    width = '(width 0.25)'.format()
    layer = '(layer {})'.format(layer)
    net = '(net {})'.format(net_id)
    segment = '(segment {} {} {} {} {})'.format(start, end, width, layer, net)
    return segment
  

def columns(sd):
    switch_x = 58.223
    switch_y = 40.822
    pad_x = 54.911949
    pad_y = 37.658989
    dx = pad_x - switch_x
    dy = pad_y - switch_y

    key_pairs = [
        ("k00", "k73"), ("k01", "k74"), ("k02", "k75"),
        ("k03", "k76"), ("k04", "k77"), ("k05", "k78"),
    ]

    i = 0
    for (a, b) in key_pairs:
        key_a = sd.get_switch_by_id(a)
        key_b = sd.get_switch_by_id(b)
        start = Point(key_a["x"] + dx, key_a["y"] + dy)
        end = Point(key_b["x"] + dx, key_b["y"] + dy)
        print(format_segment(start, end, "F.Cu", net_ids['/col{}'.format(i)]))
        i = i + 1

    switch_x = 180.95
    switch_y = 63.773
    pad_x = 177.14
    pad_y = 61.233
    dx = pad_x - switch_x
    dy = pad_y - switch_y

    key_pairs = [
        ("k06", "k64"), ("k07", "k65"), ("k08", "k66"),
    ]

    key = sd.get_switch_by_id("k64")
    start = Point(key["x"] + dx, key["y"] + dy)
    movements = [
            (5.08,      math.radians(270)),
            (16.51,     math.radians(0)),
            (19.685,    math.radians(270)),
            (32.392,    math.radians(0)),
            (14.754,    math.radians(270)),
            (48.902,    math.radians(180)),
            (34.38,     math.radians(270)),
            (48.902,    math.radians(0)),
            (14.119,    math.radians(270)),
            (48.902,    math.radians(180)),
            (8.98,      math.radians(270)),
    ]
    build_path(start, movements).print_segments("F.Cu", net_ids['/col{}'.format(i)])
    i = i + 1

    key = sd.get_switch_by_id("k65")
    start = Point(key["x"] + dx, key["y"] + dy)
    movements = [
            (24.13,     math.radians(270)),
            (30.487,    math.radians(0)),
            (16.024,    math.radians(270)),
            (30.487,    math.radians(180)),
            (33.11,     math.radians(270)),
            (30.487,    math.radians(0)),
            (15.389,    math.radians(270)),
            (30.487,    math.radians(180)),
            (8.345,     math.radians(270)),
    ]
    build_path(start, movements).print_segments("F.Cu", net_ids['/col{}'.format(i)])
    i = i + 1

    key = sd.get_switch_by_id("k66")
    start = Point(key["x"] + dx, key["y"] + dy)
    movements = [
            (5.08,      math.radians(270)),
            (12.0725,   math.radians(0)),
            (35.709,    math.radians(270)),
            (12.0725,   math.radians(180)),
            (31.84,     math.radians(270)),
            (12.0725,   math.radians(0)),
            (16.659,    math.radians(270)),
            (12.0725,   math.radians(180)),
            (7.71,      math.radians(270)),
    ]
    build_path(start, movements).print_segments("F.Cu", net_ids['/col{}'.format(i)])
    i = i + 1

    switch_x = 265.035
    switch_y = 154.119
    pad_x = 260.841816
    pad_y = 152.279188
    dx = pad_x - switch_x
    dy = pad_y - switch_y

    key_pairs = [
        ("k09", "k81"), ("k10", "k82"), ("k11", "k83"),
        ("k12", "k84"), ("k13", "k85"), ("k14", "k86"),
    ]

    for (a, b) in key_pairs:
        key_a = sd.get_switch_by_id(a)
        key_b = sd.get_switch_by_id(b)
        start = Point(key_a["x"] + dx, key_a["y"] + dy)
        end = Point(key_b["x"] + dx, key_b["y"] + dy)
        print(format_segment(start, end, "F.Cu", net_ids['/col{}'.format(i)]))
        i = i + 1

def switch_to_diodes(sd):
    d = 5.7225

    pad_a_x = 61.606544
    pad_a_y = 36.260243
    switch_x = 58.223
    switch_y = 40.822
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y

    keys = [
        "00", "01", "02", "03", "04", "05",
        "15", "16", "17", "18", "19", "20",
        "30", "31", "32", "33", "34", "35",
        "45", "46", "47", "48", "49", "50",
        "58", "59", "60", "61", "62", "63",
        "73", "74", "75", "76", "77", "78", "79",
    ]

    for key in keys:
        net_id = net_ids['"Net-(D{}-Pad2)"'.format(key)]
        k = sd.get_switch_by_id("k{}".format(key))
        start = Point(k["x"] + dxa, k["y"] + dya)
        movements = [
            [ (4.5425,  math.radians(10))  ],
            [ (1.18,  math.radians(10)), (1.18,  math.radians(100))   ],
        ]
        build_path_2(start, movements).print_segments("B.Cu", net_id)

    pad_a_x = 183.49
    pad_a_y = 58.693
    switch_x = 180.95
    switch_y = 63.773
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y

    keys = [
        "06", "07", "08",
        "21", "22", "23",
        "36", "37", "38",
              "51",
        "64", "65", "66",
    ]

    for key in keys:
        net_id = net_ids['"Net-(D{}-Pad2)"'.format(key)]
        k = sd.get_switch_by_id("k{}".format(key))
        start = Point(k["x"] + dxa, k["y"] + dya)
        movements = [
            [ (4.5425,  math.radians(0))  ],
            [ (1.18,  math.radians(0)), (1.18,  math.radians(90))   ],
        ]
        build_path_2(start, movements).print_segments("B.Cu", net_id)

    pad_a_x = 250.114279
    pad_a_y = 54.87211
    switch_x = 248.495
    switch_y = 60.316
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y
    d = 10.8025

    keys = [
              "09", "10", "11", "12", "13", "14",
              "24", "25", "26", "27", "28", "29",
              "39", "40", "41", "42", "43", "44",
              "52", "53", "54", "55", "56", "57",
              "67", "68", "69", "70", "71", "72",
        "80", "81", "82", "83", "84", "85", "86",
    ]

    for key in keys:
        net_id = net_ids['"Net-(D{}-Pad2)"'.format(key)]
        k = sd.get_switch_by_id("k{}".format(key))
        start = Point(k["x"] + dxa, k["y"] + dya)
        movements = [
            [ (9.6225,  math.radians(170))  ],
            [ (1.18,  math.radians(80)), (1.18,  math.radians(170))   ],
        ]
        build_path_2(start, movements).print_segments("B.Cu", net_id)

def diode_rows(sd):
    # Left Side
    rows = [
        (net_ids['/row0'], Point(65.682746, 46.097518)),
        (net_ids['/row1'], Point(62.374746, 64.857518)),
        (net_ids['/row2'], Point(59.066746, 83.618518)),
        (net_ids['/row3'], Point(55.758746, 102.378518)),
        (net_ids['/row4'], Point(52.450746, 121.139518)),
        (net_ids['/row5'], Point(49.142746, 139.899518)),
    ]

    for (net_id, start) in rows:
        # TODO: Need 1.27 extra in X dimension
        movements = [
            #[ (19.05 + 1.27,  math.radians(10))  ],  # p1
            [ (19.05,  math.radians(10))  ],  # p1
            [ (3.0, math.radians(280)), (3.0, math.radians(10)) ], # p2
            [ (19.05 - 3.0,         math.radians(10))  ],  # p3
            [ (5.0, math.radians(280)), (5.0, math.radians(10)) ], # p4
            [ (19.05 - 5.0,         math.radians(10))  ],  # p5
            [ (6.0, math.radians(100)), (6.0, math.radians(10)) ], # p6
            [ (19.05 - 6.0,         math.radians(10))  ],  # p7
            [ (5.0, math.radians(100)), (5.0, math.radians(10)) ], # p8
        ]
        # The last row has an extra key
        if net_id == net_ids['/row5']:
            # TODO: Do this
            movements.append([ ((19.05 * 2.0) - 5.0,  math.radians(10)) ])  # p9
        else:
            movements.append([ (19.05 - 5.0,  math.radians(10))])  # p9
        build_path_2(start, movements).print_segments("B.Cu", net_id)
    
    # Middle
    rows = [
        (net_ids['/row0'], Point(189.212499, 67.673)),
        (net_ids['/row1'], Point(189.212499, 97.122)),
        (net_ids['/row2'], Point(189.212499, 116.172)),
        (net_ids['/row4'], Point(189.212499, 164.671)),
    ]

    for (net_id, start) in rows:
        path = build_path(start, [ (19.05 * 2.0,  math.radians(0)) ])
        path.print_segments("B.Cu", net_id)

    # Right
    rows = [
        (net_ids['/row0'], Point(334.317253, 46.097518)),
        (net_ids['/row1'], Point(337.625253, 64.857518)),
        (net_ids['/row2'], Point(340.933253, 83.618518)),
        (net_ids['/row3'], Point(344.241253, 102.378518)),
        (net_ids['/row4'], Point(347.549253, 121.139518)),
        (net_ids['/row5'], Point(350.857253, 139.899518)),
    ]

    for (net_id, start) in rows:
        movements = [
                [ (19.05,  math.radians(170)) ], # p1
                [ (3.0, math.radians(260)), (3.0, math.radians(170)) ], # p2
                [ (19.05 - 3.0,         math.radians(170)) ], # p3
                [ (5.0, math.radians(260)), (5.0, math.radians(170)) ], # p4
                [ (19.05 - 5.0,         math.radians(170)) ], # p5
                [ (6.0, math.radians(80)), (6.0, math.radians(170))  ], # p6
                [ (19.05 - 6.0,         math.radians(170)) ], # p7
                [ (5.0, math.radians(80)), (5.0, math.radians(170)) ], # p8
        ]
        # The last row has an extra key
        if net_id == net_ids['/row5']:
            movements.append([((19.05 * 2.0) - 5.0,  math.radians(170))])  # p9
        else:
            movements.append([(19.05 - 5.0,  math.radians(170))])  # p9
        build_path_2(start, movements).print_segments("B.Cu", net_id)

def columns_to_center(sd):
    # To keep traces 0.635 mm apart when moving them at a 45* angle this is the
    # extra distance required per track in the X dimension.
    angle_dx = math.tan(math.radians(45.0 / 2.0)) * 0.635

    # This defines how far to extend the final trace into the middle area
    extra_dx = 5.0874990000000135 - 1.905

    net_id = net_ids['/col0']
    start = Point(48.295949, 75.179989).project(12.065 - (0.635 / 2.0) - (0.635 * 2.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) + 19.05, math.radians(10.0)) ],
        [ (3.0, math.radians(10.0)), (3.0, math.radians(280)) ],
        [ (19.05 - 3.0, math.radians(10.0)) ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(280)) ],
        [ (19.05 - 5.0, math.radians(10.0)), ],
        [ (6.0, math.radians(10.0)), (6.0, math.radians(100)) ],
        [ (19.05 - 6.0, math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(100)) ],
        [ (19.05 - 5.0 + extra_dx, math.radians(10.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col1']
    start = Point(67.056949, 78.487989).project(12.065 - (0.635 / 2.0) - (0.635 * 1.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) + (1.0 * angle_dx), math.radians(10.0)), ],
        [ (3.0, math.radians(10.0)), (3.0, math.radians(280)) ],
        [ (19.05 - 3.0, math.radians(10.0)) ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(280)) ],
        [ (19.05 - 5.0 - (2.0 * angle_dx), math.radians(10.0)), ],
        [ (6.0, math.radians(10.0)), (6.0, math.radians(100)) ],
        [ (19.05 - 6.0, math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(100)) ],
        [ (19.05 - 5.0 + (1.0 * angle_dx) + extra_dx, math.radians(10.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col2']
    start = Point(86.337949, 78.840989).project(12.065 - (0.635 / 2.0) - (0.635 * 0.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) + (2.0 * angle_dx), math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(280)) ],
        [ (19.05 - 5.0 - (4.0 * angle_dx), math.radians(10.0)), ],
        [ (6.0, math.radians(10.0)), (6.0, math.radians(100)) ],
        [ (19.05 - 6.0, math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(100)) ],
        [ (19.05 - 5.0 + (2.0 * angle_dx) + extra_dx, math.radians(10.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col3']
    start = Point(105.966949, 77.224989).project(12.065 + (0.635 / 2.0) + (0.635 * 0.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) - (3.0 * angle_dx), math.radians(10.0)), ],
        [ (6.0, math.radians(10.0)), (6.0, math.radians(100)) ],
        [ (19.05 - 6.0, math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(100)) ],
        [ (19.05 - 5.0 + (3.0 * angle_dx) + extra_dx, math.radians(10.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col4']
    start = Point(123.685949, 86.441989).project(12.065 + (0.635 / 2.0) + (0.635 * 1.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) - (4.0 * angle_dx), math.radians(10.0)), ],
        [ (5.0, math.radians(10.0)), (5.0, math.radians(100)) ],
        [ (19.05 - 5.0 + (4.0 * angle_dx) + extra_dx, math.radians(10.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col5']
    start = Point(141.577949, 94.673989).project(12.065 + (0.635 / 2.0) + (0.635 * 2.0) + 0.635, math.radians(100))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)), ],
        [ ((13.335 - 0.635) + extra_dx, math.radians(10.0)), ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col14']
    start = Point(344.199816, 76.503188).project(12.065 - (0.635 / 2.0) - (0.635 * 2.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) + 19.05, math.radians(170.0)) ],
        [ (3.0, math.radians(170.0)), (3.0, math.radians(260.0)) ],
        [ (19.05 - 3.0, math.radians(170.0)) ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(260.0)) ],
        [ (19.05 - 5.0, math.radians(170.0)), ],
        [ (6.0, math.radians(170.0)), (6.0, math.radians(80.0)) ],
        [ (19.05 - 6.0, math.radians(170.0)), ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(80.0)) ],
        [ (19.05 - 5.0 + extra_dx, math.radians(170.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col13']
    start = Point(325.438816, 79.811188).project(12.065 - (0.635 / 2.0) - (0.635 * 1.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) + (1.0 * angle_dx), math.radians(170.0)) ],
        [ (3.0, math.radians(170.0)), (3.0, math.radians(260.0)) ],
        [ (19.05 - 3.0, math.radians(170.0)) ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(260.0)) ],
        [ (19.05 - 5.0 - (2.0 * angle_dx), math.radians(170.0)), ],
        [ (6.0, math.radians(170.0)), (6.0, math.radians(80.0)) ],
        [ (19.05 - 6.0, math.radians(170.0)), ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(80.0)) ],
        [ (19.05 - 5.0 + (1.0 * angle_dx) + extra_dx, math.radians(170.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col12']
    start = Point(306.157816, 80.164188).project(12.065 - (0.635 / 2.0) - (0.635 * 0.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) + (2.0 * angle_dx), math.radians(170.0)) ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(260.0)) ],
        [ (19.05 - 5.0 - (4.0 * angle_dx), math.radians(170.0)), ],
        [ (6.0, math.radians(170.0)), (6.0, math.radians(80.0)) ],
        [ (19.05 - 6.0, math.radians(170.0)), ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(80.0)) ],
        [ (19.05 - 5.0 + (2.0 * angle_dx) + extra_dx, math.radians(170.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col11']
    start = Point(286.528816, 78.548188).project(12.065 + (0.635 / 2.0) + (0.635 * 0.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) - (3.0 * angle_dx), math.radians(170.0)) ],
        [ (6.0, math.radians(170.0)), (6.0, math.radians(80.0)) ],
        [ (19.05 - 6.0, math.radians(170.0)), ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(80.0)) ],
        [ (19.05 - 5.0 + (3.0 * angle_dx) + extra_dx, math.radians(170.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col10']
    start = Point(268.809816, 87.765188).project(12.065 + (0.635 / 2.0) + (0.635 * 1.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) - (4.0 * angle_dx), math.radians(170.0)) ],
        [ (5.0, math.radians(170.0)), (5.0, math.radians(80.0)) ],
        [ (19.05 - 5.0 + (4.0 * angle_dx) + extra_dx, math.radians(170.0)), ]
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    net_id = net_ids['/col9']
    start = Point(250.917816, 95.997188).project(12.065 + (0.635 / 2.0) + (0.635 * 2.0) + 0.635, math.radians(80))
    print(format_via(start.x, start.y, net_id))
    movements = [
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)), ],
        [ ((5.715 - 0.635) + extra_dx, math.radians(170.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)


def rows_to_center(sd):
    # Distance to change in y axis to account for switching the start point
    # from SMD pad to through-hole pad
    dy = 2.5

    # Left side
    net_id = net_ids['/row0']
    start = Point(158.964746, 65.591518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(100.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(100.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (12.3325 - 0.635 - (2.0 * 0.635) + (2.0 * ANGLE_DELTA) - 0.635, math.radians(100)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (0.635 + (0.0 * ANGLE_DELTA), math.radians(10)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row1']
    start = Point(155.656746, 84.352518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(100.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (12.3325 - 0.635 - (1.0 * 0.635) + (1.0 * ANGLE_DELTA) - 1.27 + ANGLE_DELTA, math.radians(100)) ],
        [ (1.27 - ANGLE_DELTA, math.radians(10.0)), (1.27 - ANGLE_DELTA, math.radians(100.0)) ],
        [ (0.635 + (1.0 * ANGLE_DELTA), math.radians(10)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row2']
    start = Point(152.348746, 103.112518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(100.0)) ],
        [ (12.3325 - 0.635 - (3.0 * 0.635) + (2.0 * ANGLE_DELTA), math.radians(100)) ],
        [
            ((0.635 * 3.0) - (ANGLE_DELTA * 2.0), math.radians(10.0)),
            ((0.635 * 3.0) - (ANGLE_DELTA * 2.0), math.radians(100.0)) ],
        [ (0.635 + (2.0 * ANGLE_DELTA), math.radians(10)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row3']
    start = Point(149.040746, 121.873518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (6.0825 - 0.635 - (0.635 * 3.0) + (2.0 * ANGLE_DELTA), math.radians(280)) ],
        [
            ((3.0 * 0.635) - (2.0 * ANGLE_DELTA), math.radians(10.0)),
            ((3.0 * 0.635) - (2.0 * ANGLE_DELTA), math.radians(280.0)),
        ],
        [ (0.635 + (2.0 * ANGLE_DELTA), math.radians(10.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row4']
    start = Point(145.732746, 140.633518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(280.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (6.0825 - 0.635 - (1.0 * 0.635) + (1.0 * ANGLE_DELTA) - (0.635 * 2.0) + ANGLE_DELTA, math.radians(280.0)) ],
        [ ((2.0 * 0.635) - (1.0 * ANGLE_DELTA), math.radians(10.0)), ((2.0 * 0.635) - (1.0 * ANGLE_DELTA), math.radians(280.0)) ],
        [ (0.635 + (1.0 * ANGLE_DELTA), math.radians(10.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row5']
    start = Point(142.424746, 159.394518)
    movements = [
        [ (1.27, math.radians(10.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(280.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(280.0)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (6.0825 - 0.635 - (2.0 * 0.635) + (2.0 * ANGLE_DELTA) - 0.635, math.radians(280)) ],
        [ (0.635, math.radians(10.0)), (0.635, math.radians(280.0)) ],
        [ (0.635 + (0.0 * ANGLE_DELTA), math.radians(10.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    # Right side
    net_id = net_ids['/row0']
    start = Point(241.035253, 65.591518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(80.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(80.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (12.3325 - 0.635 - (2.0 * 0.635) + (2.0 * ANGLE_DELTA) - 0.635, math.radians(80)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (0.635 + (0.0 * ANGLE_DELTA), math.radians(170)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row1']
    start = Point(244.343253, 84.352518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(80.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (12.3325 - 0.635 - (1.0 * 0.635) + (1.0 * ANGLE_DELTA) - 1.27 + ANGLE_DELTA, math.radians(80)) ],
        [ (1.27 - ANGLE_DELTA, math.radians(170.0)), (1.27 - ANGLE_DELTA, math.radians(80.0)) ],
        [ (0.635 + (1.0 * ANGLE_DELTA), math.radians(170)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row2']
    start = Point(247.651253, 103.112518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(80.0)) ],
        [ (12.3325 - 0.635 - (3.0 * 0.635) + (2.0 * ANGLE_DELTA), math.radians(80)) ],
        [
            ((0.635 * 3.0) - (ANGLE_DELTA * 2.0), math.radians(170.0)),
            ((0.635 * 3.0) - (ANGLE_DELTA * 2.0), math.radians(80.0)) ],
        [ (0.635 + (2.0 * ANGLE_DELTA), math.radians(170)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row3']
    start = Point(250.959253, 121.873518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (6.0825 - 0.635 - (0.635 * 3.0) + (2.0 * ANGLE_DELTA), math.radians(260)) ],
        [
            ((3.0 * 0.635) - (2.0 * ANGLE_DELTA), math.radians(170.0)),
            ((3.0 * 0.635) - (2.0 * ANGLE_DELTA), math.radians(260.0)),
        ],
        [ (0.635 + (2.0 * ANGLE_DELTA), math.radians(170.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row4']
    start = Point(254.267253, 140.633518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(260.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (6.0825 - 0.635 - (1.0 * 0.635) + (1.0 * ANGLE_DELTA) - (0.635 * 2.0) + ANGLE_DELTA, math.radians(260.0)) ],
        [ ((2.0 * 0.635) - (1.0 * ANGLE_DELTA), math.radians(170.0)), ((2.0 * 0.635) - (1.0 * ANGLE_DELTA), math.radians(260.0)) ],
        [ (0.635 + (1.0 * ANGLE_DELTA), math.radians(170.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)

    net_id = net_ids['/row5']
    start = Point(257.575253, 159.394518)
    movements = [
        [ (1.27, math.radians(170.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(260.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (19.05 - 0.635 - ANGLE_DELTA, math.radians(260.0)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (6.0825 - 0.635 - (2.0 * 0.635) + (2.0 * ANGLE_DELTA) - 0.635, math.radians(260)) ],
        [ (0.635, math.radians(170.0)), (0.635, math.radians(260.0)) ],
        [ (0.635 + (0.0 * ANGLE_DELTA), math.radians(170.0)) ],
    ]
    build_path_2(start, movements).print_segments("F.Cu", net_id)


def rows_clusters(sd):
    '''Connect the rows in left cluster to center and right cluster to center.

    This is commented out in main because I Changed my mind about doing this
    because it looks weird. I'll just wire these up by hand instead.
    '''

    # Row 0 center cluster to left cluster
    net_id = net_ids["/row0"]
    p1 = Point(189.212499, 67.673)
    theta1 = math.radians(180.0)
    p2 = Point(158.964746, 65.591518)
    theta2 = math.radians(10.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    # Row 0 center cluster to right cluster
    net_id = net_ids["/row0"]
    p1 = Point(227.3125, 67.673)
    theta1 = math.radians(0.0)
    p2 = Point(241.035253, 65.591518)
    theta2 = math.radians(170.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    # Row 1 center cluster to left cluster
    net_id = net_ids["/row1"]
    start = Point(189.212499, 97.122)
    movements = [
        [ ((8.2625 * 2.0) + 1.27, math.radians(180.0)) ],
        [ (0.635, math.radians(180.0)), (0.635, math.radians(270.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    p1 = Point(170.7825, 96.487)
    theta1 = math.radians(270.0)
    p2 = Point(155.656746, 84.352518)
    theta2 = math.radians(10.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    # Row 1 center cluster to right cluster
    net_id = net_ids["/row1"]
    start = Point(227.3125, 97.122)
    movements = [
        [ (1.27, math.radians(0.0)) ],
        [ (0.635, math.radians(0.0)), (0.635, math.radians(270.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    p1 = Point(229.2175, 96.487)
    theta1 = math.radians(270.0)
    p2 = Point(244.343253, 84.352518)
    theta2 = math.radians(170.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    # Row 2 center cluster to left cluster
    net_id = net_ids["/row1"]
    start = Point(189.212499, 116.172)
    movements = [
        [ ((8.2625 * 2.0) + 1.27, math.radians(180.0)) ],
        [ (0.635, math.radians(180.0)), (0.635, math.radians(270.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    p1 = Point(170.7825, 115.537)
    theta1 = math.radians(270.0)
    p2 = Point(152.348746, 103.112518)
    theta2 = math.radians(10.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    # Row 2 center cluster to right cluster
    net_id = net_ids["/row1"]
    start = Point(227.3125, 116.172)
    movements = [
        [ (1.27, math.radians(0.0)) ],
        [ (0.635, math.radians(0.0)), (0.635, math.radians(270.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    p1 = Point(229.2175, 115.537)
    theta1 = math.radians(270.0)
    p2 = Point(247.651253, 103.112518)
    theta2 = math.radians(170.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))


def leds_to_caps(sd):
    net_id = net_ids["VCC"]

    path = Path([Point(252.283942, 67.69587), Point(254.785262, 67.25516)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(287.894942, 50.24687), Point(290.396262, 49.80616)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(326.804942, 51.50887), Point(329.306262, 51.06816)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(335.767058, 130.553129), Point(333.265738, 130.99384)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(296.857058, 129.29113), Point(294.355738, 129.73184)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(261.246057, 146.73913), Point(258.744738, 147.17984)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(133.928384, 145.888254), Point(131.426738, 145.44716)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(98.317384, 128.440254), Point(95.815738, 127.99916)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(59.407384, 129.702253), Point(56.905738, 129.261159)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(78.020616, 52.359746), Point(80.522262, 52.80084)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(116.930616, 51.097746), Point(119.432262, 51.53884)])
    path.print_segments("B.Cu", net_id)

    path = Path([Point(152.541616, 68.546746), Point(155.043262, 68.98784)])
    path.print_segments("B.Cu", net_id)


def center_vias(sd):
    via_dx = 42.54
    via_dy_initial = 106.68

    # Left
    net_id = net_ids["/col0"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 0.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(156.02511, 107.86096)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(135.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col1"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 1.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(155.91525, 108.48631)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(135.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col2"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 2.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(155.80445, 109.11109)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col3"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 3.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(155.69436, 109.73649)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col4"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 4.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(155.58439, 110.36199)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col5"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 5.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(155.47378, 110.98731)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/row0"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 6.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.80524, 114.779)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(135.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row1"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 7.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.69497, 115.40476)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(135.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row2"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 8.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.5847, 116.02953)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row3"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + ( 9.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.47443, 116.65529)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row4"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + (10.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.36416, 117.28006)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row5"]
    (via_x, via_y) = (200.0 - via_dx, via_dy_initial + (11.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(154.25389, 117.90582)
    theta1 = math.radians(0.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(225.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    # Right
    net_id = net_ids["/col14"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 0.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(243.97489, 107.86096)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(45.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col13"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 1.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(244.08475, 108.48631)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(45.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col12"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 2.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(244.19555, 109.11109)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col11"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 3.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(244.30564, 109.73649)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col10"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 4.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(244.41561, 110.36199)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/col9"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 5.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(244.52622, 110.98731)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "B.Cu", net_id))
    print(format_segment(i, p2, "B.Cu", net_id))

    net_id = net_ids["/row0"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 6.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.19476, 114.779)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(45.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row1"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 7.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.30503, 115.40476)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(45.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row2"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 8.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.4153, 116.02953)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row3"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + ( 9.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.52557, 116.65529)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row4"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + (10.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.63584, 117.28006)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))

    net_id = net_ids["/row5"]
    (via_x, via_y) = (200.0 + via_dx, via_dy_initial + (11.0 * 1.27))
    print(format_via(via_x, via_y, net_id))
    p1 = Point(245.74611, 117.90582)
    theta1 = math.radians(180.0)
    p2 = Point(via_x, via_y)
    theta2 = math.radians(315.0)
    i = intersection(p1, theta1, p2, theta2)
    print(format_segment(p1, i, "F.Cu", net_id))
    print(format_segment(i, p2, "F.Cu", net_id))


def leds_vcc_data(sd):
    # Notes:
    # The distance from the VCC pad on the LED footprint to the VCC pad on the
    # capacitor footprint is 2.54mm

    TODO_1 = 1.3675
    TODO_2 = 1.3675

    # LED1 VCC to LED2 VCC
    net_id = net_ids["VCC"]
    start = Point(252.283942, 67.69487)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (1.3675, math.radians(80.0)), ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (0.635 + 0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(260.0)) ],
        [ (1.27 + ANGLE_DELTA, math.radians(260.0)) ],
        [ ((2.0 * 0.635) - ANGLE_DELTA, math.radians(350.0)), ((2.0 * 0.635) - ANGLE_DELTA, math.radians(260.0)) ],
        [ (4.0, math.radians(350.0)) ],
        [ (3.18125, math.radians(350.0)), (3.18125, math.radians(260.0)) ],
        [ (11.0 + ANGLE_DELTA, math.radians(350.0)) ],
        [ (6.0 + 1.28125 + 1.27, math.radians(350.0)), (6.0 + 1.28125 + 1.27, math.radians(260.0)) ],
        [ (4.0 + 6.2875 - 2.71875 - 1.27 - 1.28125, math.radians(350.0)) ],
        [ (1.27, math.radians(350.0)), (1.27, math.radians(80.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    # LED1 data to LED2 data
    net_id = net_ids['"Net-(LED1-Pad2)"']
    start = Point(252.839616, 70.847254)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (2.54, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(260.0)) ],
        [ (1.1975 + 1.27, math.radians(260.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(260.0)) ],
        [ (4.0, math.radians(350.0)) ],
        [ (3.18125, math.radians(350.0)), (3.18125, math.radians(260.0)) ],
        [ (11.0 + ANGLE_DELTA, math.radians(350.0)) ],
        [ (6.0 + 1.91625, math.radians(350.0)), (6.0 + 1.91625, math.radians(260.0)) ],
        [ (1.75947, math.radians(350.0)) ],
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    # LED2 VCC to LED3 VCC
    net_id = net_ids["VCC"]
    start = Point(287.894942, 50.24587)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (1.3675, math.radians(80.0)), ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (1.27, math.radians(350.0)) ],
        [ (0.5625, math.radians(350.0)), (0.5625, math.radians(80.0)) ],
        [ (0.635, math.radians(350.0)) ],

        # TODO: Connect to LED3
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    # LED2 data to LED3 data
    net_id = net_ids['"Net-(LED2-Pad2)"']
    start = Point(288.450616, 53.398254)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (2.4675, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)) ],

        # TODO: Connect to LED3
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    # LED3 VCC to LED4 VCC
    net_id = net_ids["VCC"]
    start = Point(326.804942, 51.50887)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (1.3675, math.radians(80.0)), ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(260.0)) ],
        [ (TODO_1, math.radians(260.0)), ],
        

        # TODO: Connect to LED4
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

    # LED3 data to LED4 data
    net_id = net_ids['"Net-(LED3-Pad2)"']
    start = Point(327.360616, 54.660254)
    movements = [
        [ (0.635, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(80.0)) ],
        [ (2.54, math.radians(350.0)) ],
        [ (0.635, math.radians(350.0)), (0.635, math.radians(260.0)) ],
        [ (TODO_2, math.radians(260.0)), ],

        # TODO: Connect to LED4
    ]
    build_path_2(start, movements).print_segments("B.Cu", net_id)

def generate_traces(args: argparse.Namespace) -> int:
    sd = SwitchData()
    columns(sd)
    switch_to_diodes(sd)
    diode_rows(sd)
    columns_to_center(sd)

    rows_to_center(sd)
    #rows_clusters(sd) # This is intentionally removed because I don't like it
    #center_vias(sd) # TODO: Uncomment, it's just slow so I'm removing while iterating elsewhere

    leds_vcc_data(sd)

    # TODO: Connect LED data/VCC lines

    return 0
