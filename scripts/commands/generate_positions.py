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


def add_generate_positions_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "gen-positions", help="Generate positions for PCB components"
    )
    parser.set_defaults(func=generate_positions)


class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    @staticmethod
    def midpoint(p1, p2):
        return Point(p1.x + ((p2.x - p1.x) / 2.0), p1.y + ((p2.y - p1.y) / 2.0))

    @staticmethod
    def from_switch(switch):
        return Point(switch["x"], switch["y"])

    def project(self, distance, theta):
        return Point(
            self.x + distance * math.cos(theta), self.y + distance * math.sin(theta)
        )


def generate_led_positions(sd):
    return {
        "LED1": (
            Point.from_switch(sd.get_switch_by_id("k09")).project(
                9.525, math.radians(80)
            ),
            190,
        ),
        "LED2": (
            Point.from_switch(sd.get_switch_by_id("k11")).project(
                9.525, math.radians(80)
            ),
            190,
        ),
        "LED3": (
            Point.from_switch(sd.get_switch_by_id("k13")).project(
                9.525, math.radians(80)
            ),
            190,
        ),
        "LED4": (
            Point.from_switch(sd.get_switch_by_id("k71")).project(
                9.525, math.radians(80)
            ),
            10,
        ),
        "LED5": (
            Point.from_switch(sd.get_switch_by_id("k69")).project(
                9.525, math.radians(80)
            ),
            10,
        ),
        "LED6": (
            Point.from_switch(sd.get_switch_by_id("k67")).project(
                9.525, math.radians(80)
            ),
            10,
        ),
        "LED7": (
            Point.from_switch(sd.get_switch_by_id("k63")).project(
                9.525, math.radians(100)
            ),
            350,
        ),
        "LED8": (
            Point.from_switch(sd.get_switch_by_id("k61")).project(
                9.525, math.radians(100)
            ),
            350,
        ),
        "LED9": (
            Point.from_switch(sd.get_switch_by_id("k59")).project(
                9.525, math.radians(100)
            ),
            350,
        ),
        "LED10": (
            Point.from_switch(sd.get_switch_by_id("k01")).project(
                9.525, math.radians(100)
            ),
            170,
        ),
        "LED11": (
            Point.from_switch(sd.get_switch_by_id("k03")).project(
                9.525, math.radians(100)
            ),
            170,
        ),
        "LED12": (
            Point.from_switch(sd.get_switch_by_id("k05")).project(
                9.525, math.radians(100)
            ),
            170,
        ),
    }


def generate_led_capacitor_positions(led_positions):
    return {
        "C9": (
            (
                led_positions["LED1"][0]
                .project(2.45 + 2.54, math.radians(350.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(350))
                #.project(1.6, math.radians(260))
            ),
            280.0,
        ),
        "C10": (
            (
                led_positions["LED2"][0]
                .project(2.45 + 2.54, math.radians(350.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(350))
                #.project(1.6, math.radians(260))
            ),
            280.0,
        ),
        "C11": (
            (
                led_positions["LED3"][0]
                .project(2.45 + 2.54, math.radians(350.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(350))
                #.project(1.6, math.radians(260))
            ),
            280.0,
        ),
        "C12": (
            (
                led_positions["LED4"][0]
                .project(2.45 + 2.54, math.radians(170.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(170))
                #.project(1.6, math.radians(80))
            ),
            100.0,
        ),
        "C13": (
            (
                led_positions["LED5"][0]
                .project(2.45 + 2.54, math.radians(170.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(170))
                #.project(1.6, math.radians(80))
            ),
            100.0,
        ),
        "C14": (
            (
                led_positions["LED6"][0]
                .project(2.45 + 2.54, math.radians(170.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(170))
                #.project(1.6, math.radians(80))
            ),
            100.0,
        ),
        "C15": (
            (
                led_positions["LED7"][0]
                .project(2.45 + 2.54, math.radians(190.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(190))
                #.project(1.6, math.radians(100))
            ),
            80.0,
        ),
        "C16": (
            (
                led_positions["LED8"][0]
                .project(2.45 + 2.54, math.radians(190.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(190))
                #.project(1.6, math.radians(100))
            ),
            80.0,
        ),
        "C17": (
            (
                led_positions["LED9"][0]
                .project(2.45 + 2.54, math.radians(190.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(190))
                #.project(1.6, math.radians(100))
            ),
            80.0,
        ),
        "C18": (
            (
                led_positions["LED10"][0]
                .project(2.45 + 2.54, math.radians(10.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(10))
                #.project(1.6, math.radians(280))
            ),
            260.0,
        ),
        "C19": (
            (
                led_positions["LED11"][0]
                .project(2.45 + 2.54, math.radians(10.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(10))
                #.project(1.6, math.radians(280))
            ),
            260.0,
        ),
        "C20": (
            (
                led_positions["LED12"][0]
                .project(2.45 + 2.54, math.radians(10.0))
                #.project(2.45 + 2.54 + 1.0375, math.radians(10))
                #.project(1.6, math.radians(280))
            ),
            260.0,
        ),
    }

def generate_spacer_positions(sd):
    return {
        "H1": (
            (
                Point.from_switch(sd.get_switch_by_id("k25"))
                .project(9.525, math.radians(170))
                .project(9.525, math.radians(260))
            ),
            0,
        ),
        "H2": (
            (
                Point.from_switch(sd.get_switch_by_id("k29"))
                .project(9.525, math.radians(170))
                .project(9.525, math.radians(260))
            ),
            0,
        ),
        "H3": (
            (
                Point.from_switch(sd.get_switch_by_id("k86"))
                .project(9.525, math.radians(170))
                .project(9.525, math.radians(260))
            ),
            0,
        ),
        "H4": (
            (
                Point.from_switch(sd.get_switch_by_id("k82"))
                .project(9.525, math.radians(170))
                .project(9.525, math.radians(260))
            ),
            0,
        ),
        "H5": (
            (
                Point.from_switch(sd.get_switch_by_id("k77"))
                .project(9.525, math.radians(10))
                .project(9.525, math.radians(280))
            ),
            0,
        ),
        "H6": (
            (
                Point.from_switch(sd.get_switch_by_id("k73"))
                .project(9.525, math.radians(10))
                .project(9.525, math.radians(280))
            ),
            0,
        ),
        "H7": (
            (
                Point.from_switch(sd.get_switch_by_id("k15"))
                .project(9.525, math.radians(10))
                .project(9.525, math.radians(280))
            ),
            0,
        ),
        "H8": (
            (
                Point.from_switch(sd.get_switch_by_id("k19"))
                .project(9.525, math.radians(10))
                .project(9.525, math.radians(280))
            ),
            0,
        ),
        "H9": (
            (
                Point.midpoint(
                    Point.from_switch(sd.get_switch_by_id("k37")),
                    Point.from_switch(sd.get_switch_by_id("k51"))
                )
            ),
            0,
        ),
    }

def generate_positions(args: argparse.Namespace) -> int:
    sd = SwitchData()

    component_positions = {}

    led_positions = generate_led_positions(sd)
    led_cap_positions = generate_led_capacitor_positions(led_positions)

    spacer_positions = generate_spacer_positions(sd)

    component_positions.update(led_positions)
    component_positions.update(led_cap_positions)
    component_positions.update(spacer_positions)

    for name in component_positions:
        (position, rotation) = component_positions[name]
        print(
            '"{}": ({}, {}, {}),'.format(
                name, round(position.x, 3), round(position.y, 3), rotation
            )
        )

    return 0
