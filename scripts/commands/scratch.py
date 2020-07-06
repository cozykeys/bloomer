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

    print("Bloomer repo directory: {}".format(bloomer_dir))

    sd = SwitchData()
    switches = sd.get_switches()

    vertices = [
        sd.get_corner((0, 0), Corner.TOP_LEFT),
        sd.get_corner((0, 3), Corner.TOP_LEFT),
        sd.get_corner((0, 3), Corner.TOP_RIGHT),
        sd.get_corner((0, 4), Corner.TOP_RIGHT),
        sd.get_corner((0, 10), Corner.TOP_LEFT),
        sd.get_corner((0, 11), Corner.TOP_LEFT),
        sd.get_corner((0, 11), Corner.TOP_RIGHT),
        sd.get_corner((0, 14), Corner.TOP_RIGHT),
        sd.get_corner((5, 14), Corner.BOTTOM_RIGHT),
        sd.get_corner((5, 13), Corner.BOTTOM_LEFT),
        sd.get_corner((5, 9), Corner.BOTTOM_RIGHT),
        sd.get_corner((4, 8), Corner.BOTTOM_LEFT),
        sd.get_corner((5, 8), Corner.BOTTOM_RIGHT),
        sd.get_corner((5, 6), Corner.BOTTOM_LEFT),
        sd.get_corner((4, 6), Corner.BOTTOM_RIGHT),
        sd.get_corner((5, 5), Corner.BOTTOM_LEFT),
        sd.get_corner((5, 1), Corner.BOTTOM_RIGHT),
        sd.get_corner((5, 0), Corner.BOTTOM_LEFT),
    ]

    print("{} -> {}".format(vertices[3], vertices[4]))

    perimeter = Polygon2D([ Vector2D(v[0], v[1]) for v in vertices ])

    svg_writer = SvgWriter()

    for switch in switches:
        poly = Polygon2D(
            [
                Vector2D(switch["x"] - 9.525, switch["y"] - 9.525),
                Vector2D(switch["x"] + 9.525, switch["y"] - 9.525),
                Vector2D(switch["x"] + 9.525, switch["y"] + 9.525),
                Vector2D(switch["x"] - 9.525, switch["y"] + 9.525),
            ]
        ).rotated_around(
            math.radians(switch["rotation"]), Vector2D(switch["x"], switch["y"])
        )
        svg_writer.append_element(poly, SvgStyle(SVG_STYLE_POLY))

    # svg_writer.append_element(perimeter.vertices[2], SvgStyle(SVG_STYLE_VEC))
    svg_writer.append_element(perimeter, SvgStyle(SVG_STYLE_POLY))

    svg_writer.write_to_file("{}/temp/render.svg".format(bloomer_dir))

    with open(os.path.join(bloomer_dir, "temp", "pcb_edge_vertices.json"), "w") as f:
        f.write(json.dumps(vertices, indent="    "))

    return 0
