#!/usr/bin/env python3

import json
import os
import math

from .common import Corner
from .util import get_bloomer_repo_dir
from .geometry import Polygon2D, Vector2D

# TODO: Document the expected format of the switch data file
# [
#   { "column":  0, "row": 0, "x": 40, "y": 20, "rotation": -10.0, "diode_position": "left" },
#   { "column":  0, "row": 1, "x": 60, "y": 20, "rotation":  10.0, "diode_position": "right" }
# ]

class SwitchData:
    def __init__(self):
        self.switch_data = None

        path = os.path.join(get_bloomer_repo_dir(), "data", "switches.json")
        with open(path, "r") as f:
            self.switch_data = json.loads(f.read())

    def get_switches(self):
        """Find all the switches
        """
        return self.switch_data

    def get_switch_by_index(self, index):
        """Find the switch by either the switch or diode reference (I.E. 'D00')
        """
        if len(self.switch_data) > index:
            return self.switch_data[index]
        return None

    def get_switch_by_id(self, id):
        """Find the switch by either the switch or diode reference (I.E. 'D00')
        """
        for s in self.switch_data:
            if s["id"] == id:
                return s
        return None

    def get_switch_by_coord(self, row, col):
        """Find the switch by its row and column
        """
        for s in self.switch_data:
            if s["row"] == row and s["column"] == col:
                return s
        return None

    def get_corner(self, coord, corner):
        """TODO
        """
        d = 9.525
        s = self.get_switch_by_coord(coord[0], coord[1])
        p = Polygon2D(
            [
                Vector2D(s["x"] - d, s["y"] - d),
                Vector2D(s["x"] + d, s["y"] - d),
                Vector2D(s["x"] + d, s["y"] + d),
                Vector2D(s["x"] - d, s["y"] + d),
            ]
        ).rotated_around(math.radians(s["rotation"]), Vector2D(s["x"], s["y"]))

        if corner == Corner.TOP_LEFT:
            return (round(p.vertices[0].x, 3), round(p.vertices[0].y, 3))
        elif corner == Corner.TOP_RIGHT:
            return (round(p.vertices[1].x, 3), round(p.vertices[1].y, 3))
        elif corner == Corner.BOTTOM_RIGHT:
            return (round(p.vertices[2].x, 3), round(p.vertices[2].y, 3))
        elif corner == Corner.BOTTOM_LEFT:
            return (round(p.vertices[3].x, 3), round(p.vertices[3].y, 3))

    def get_midpoint(self, s1_ref, s2_ref):
        """TODO
        """
        s1 = self.get_switch_by_id(s1_ref)
        s2 = self.get_switch_by_id(s2_ref)

        x = round(s1["x"] + ((s2["x"] - s1["x"]) / 2.0), 3)
        y = round(s1["y"] + ((s2["y"] - s1["y"]) / 2.0), 3)

        return (x, y)
