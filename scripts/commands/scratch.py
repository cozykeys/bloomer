#!/usr/bin/env python3

import json
import argparse
import math
from typing import List, Dict, Tuple
from lib import SvgStyle, SvgWriter, Vector2D, Segment2D, Polygon2D, get_bloomer_repo_dir, SwitchData


SVG_STYLE_POLY: Dict[str, str] = {
    "fill-opacity": "0.0",
    "stroke": "#7f007f",  # Purple
    "stroke-width": "0.25",
}
SVG_STYLE_VEC: Dict[str, str] = {
    "fill-opacity": "1.0",
    "fill": "#7f007f",  # Purple
    "stroke": "none",
}
SVG_STYLE_VEC_1: Dict[str, str] = {
    "fill-opacity": "1.0",
    "fill": "#ff0000",  # Purple
    "stroke": "none",
}
SVG_STYLE_HOLE: Dict[str, str] = {
    "fill-opacity": "1.0",
    "fill": "#007f7f",  # Teal
    "stroke": "none",
}


def add_scratch_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "scratch", help="Scratchpad, dumping ground for temporary code"
    )
    parser.set_defaults(func=scratch)


def scratch(args: argparse.Namespace) -> int:
    bloomer_dir = get_bloomer_repo_dir()

    print('Bloomer repo directory: {}'.format(bloomer_dir))

    sd = SwitchData()
    for s in sd.get_switches():
        print(s['x'])

    return 0
