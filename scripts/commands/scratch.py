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


def add_scratch_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "scratch", help="Scratchpad, dumping ground for temporary code"
    )
    parser.set_defaults(func=scratch)


def midpoint(sd, k1_id, k2_id):
    k1 = sd.get_switch_by_id(k1_id)
    k2 = sd.get_switch_by_id(k2_id)

    return (
        k1["x"] + ((k2["x"] - k1["x"]) / 2.0),
        k1["y"] + ((k2["y"] - k1["y"]) / 2.0),
    )

def scratch(args: argparse.Namespace) -> int:
    bloomer_dir = get_bloomer_repo_dir()

    sd = SwitchData()
    switches = sd.get_switches()

    locations = {
        "H1": midpoint(sd, "k25", "k10"),
        "H2": midpoint(sd, "k29", "k14"),
        "H3": midpoint(sd, "k86", "k72"),
        "H4": midpoint(sd, "k82", "k68"),
        "H5": midpoint(sd, "k77", "k62"),
        "H6": midpoint(sd, "k73", "k58"),
        "H7": midpoint(sd, "k15", "k00"),
        "H8": midpoint(sd, "k19", "k04"),
        "H9": midpoint(sd, "k51", "k37"),
    }

    for hole in locations:
        print('        "{}": ({}, {}, 0),'.format(
            hole,
            round(locations[hole][0], 3),
            round(locations[hole][1], 3),
        ))


    return 0
