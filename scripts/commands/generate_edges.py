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


class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    @staticmethod
    def from_switch(switch):
        return Point(switch["x"], switch["y"])

    def project(self, distance, theta):
        return Point(
            self.x + distance * math.cos(theta), self.y + distance * math.sin(theta)
        )


def add_generate_edges_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "gen-edges", help="Generate edge cuts for PCB components"
    )
    parser.set_defaults(func=generate_edges)


def generate_edges(args: argparse.Namespace) -> int:
    sd = SwitchData()

    # USB Protrusion
    dx = 4.32 + 1.27 - 0.3175
    dy = -6.5

    edge_vertices = [
        (
            Point.from_switch(sd.get_switch_by_id("k00"))
            .project(9.525, math.radians(190))
            .project(9.525, math.radians(280))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k03"))
            .project(9.525, math.radians(190))
            .project(9.525, math.radians(280))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k03"))
            .project(9.525, math.radians(10))
            .project(9.525, math.radians(280))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k05"))
            .project(9.525, math.radians(10))
            .project(9.525 + 5, math.radians(280))
        ),
        # TODO: USB-C protrusion

        (
            Point(
                200 - dx,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y
            )
        ),
        (
            Point(
                200 - dx,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y + dy + 0.635
            )
        ),
        (
            Point(
                200 - dx + 0.635,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y + dy
            )
        ),
        (
            Point(
                200 + dx - 0.635,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y + dy
            )
        ),
        (
            Point(
                200 + dx,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y + dy + 0.635
            )
        ),
        (
            Point(
                200 + dx,
                Point.from_switch(sd.get_switch_by_id("k05"))
                .project(9.525, math.radians(10))
                .project(9.525 + 5, math.radians(280))
                .y
            )
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k09"))
            .project(9.525, math.radians(170))
            .project(9.525 + 5, math.radians(260))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k11"))
            .project(9.525, math.radians(170))
            .project(9.525, math.radians(260))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k11"))
            .project(9.525, math.radians(350))
            .project(9.525, math.radians(260))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k14"))
            .project(9.525, math.radians(350))
            .project(9.525, math.radians(260))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k86"))
            .project(9.525, math.radians(350))
            .project(9.525, math.radians(80))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k85"))
            .project(9.525, math.radians(170))
            .project(9.525, math.radians(80))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k81"))
            .project(9.525, math.radians(350))
            .project(9.525, math.radians(80))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k80"))
            .project(9.525, math.radians(170))
            .project(9.525, math.radians(80))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k66"))
            .project(9.525, math.radians(0))
            .project(9.525, math.radians(90))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k64"))
            .project(9.525, math.radians(180))
            .project(9.525, math.radians(90))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k79"))
            .project(9.525, math.radians(10))
            .project(9.525, math.radians(100))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k78"))
            .project(9.525, math.radians(190))
            .project(9.525, math.radians(100))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k74"))
            .project(9.525, math.radians(10))
            .project(9.525, math.radians(100))
        ),
        (
            Point.from_switch(sd.get_switch_by_id("k73"))
            .project(9.525, math.radians(190))
            .project(9.525, math.radians(100))
        ),
    ]

    for vertex in edge_vertices:
        print('({},{}),'.format(round(vertex.x, 3), round(vertex.y, 3)))

    return 0
