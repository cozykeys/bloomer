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
    '"Net-(R1-Pad1)"': 114,
    '"Net-(R2-Pad2)"': 115,
    '/usb_d-': 116,
    '/usb_d+': 117,
    '/scl': 118,
    '/sda': 119,
    '"Net-(U1-Pad32)"': 120,
    '"Net-(U1-Pad42)"': 121,
    '"Net-(D00-Pad2)"': 122,
    '"Net-(J1-PadA6)"': 123,
    '"Net-(J1-PadB5)"': 124,
    '"Net-(J1-PadA8)"': 125,
    '"Net-(J1-PadA7)"': 126,
    '"Net-(J1-PadA5)"': 127,
    '"Net-(J1-PadB8)"': 128,
}

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

class Path:
    def __init__(self, points):
        self.points = points

    def print_segments(self, layer, net_id):
        for i in range(1, len(self.points)):
            p1 = self.points[i]
            p2 = self.points[i-1]
            print(format_segment(p1, p2, layer, net_id))

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

def add_generate_traces_parser(subparsers: argparse._SubParsersAction) -> None:
    parser = subparsers.add_parser(
        "gen-traces", help="Generate traces for PCB"
    )
    parser.set_defaults(func=generate_traces)


def format_segment(start, end, layer, net_id):
    start = '(start {} {})'.format(round(start.x, 3), round(start.y, 3))
    end = '(end {} {})'.format(round(end.x, 3), round(end.y, 3))
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
    pad_b_x = 67.037202
    pad_b_y = 38.416018
    switch_x = 58.223
    switch_y = 40.822
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y
    dxb = pad_b_x - switch_x
    dyb = pad_b_y - switch_y
    theta = math.radians(10)

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
        end = Point(start.x + d * math.cos(theta), start.y + d * math.sin(theta))
        print(format_segment(start, end, "B.Cu", net_id))
        start = Point(end.x, end.y)
        end = Point(k["x"] + dxb, k["y"] + dyb)
        print(format_segment(start, end, "B.Cu", net_id))

    pad_a_x = 183.49
    pad_a_y = 58.693
    pad_b_x = 189.212499
    pad_b_y = 59.873
    switch_x = 180.95
    switch_y = 63.773
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y
    dxb = pad_b_x - switch_x
    dyb = pad_b_y - switch_y
    theta = math.radians(0)

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
        end = Point(start.x + d * math.cos(theta), start.y + d * math.sin(theta))
        print(format_segment(start, end, "B.Cu", net_id))
        start = Point(end.x, end.y)
        end = Point(k["x"] + dxb, k["y"] + dyb)
        print(format_segment(start, end, "B.Cu", net_id))

    pad_a_x = 250.114279
    pad_a_y = 54.87211
    pad_b_x = 239.680797
    pad_b_y = 57.910018
    switch_x = 248.495
    switch_y = 60.316
    dxa = pad_a_x - switch_x
    dya = pad_a_y - switch_y
    dxb = pad_b_x - switch_x
    dyb = pad_b_y - switch_y
    theta = math.radians(170)
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
        end = Point(start.x + d * math.cos(theta), start.y + d * math.sin(theta))
        print(format_segment(start, end, "B.Cu", net_id))
        start = Point(end.x, end.y)
        end = Point(k["x"] + dxb, k["y"] + dyb)
        print(format_segment(start, end, "B.Cu", net_id))

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
        movements = [
                (19.05 + 1.27,  math.radians(10)),  # p1
                (3.0,           math.radians(280)), # p2
                (19.05,         math.radians(10)),  # p3
                (5.0,           math.radians(280)), # p4
                (19.05,         math.radians(10)),  # p5
                (6.0,           math.radians(100)), # p6
                (19.05,         math.radians(10)),  # p7
                (5.0,           math.radians(100)), # p8
        ]
        # The last row has an extra key
        if net_id == net_ids['/row5']:
            movements.append(((19.05 * 2.0) - 1.27,  math.radians(10)))  # p9
        else:
            movements.append((19.05 - 1.27,  math.radians(10)))  # p9
        build_path(start, movements).print_segments("B.Cu", net_id)
    
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
                (19.05 + 1.27,  math.radians(170)), # p1
                (3.0,           math.radians(260)), # p2
                (19.05,         math.radians(170)), # p3
                (5.0,           math.radians(260)), # p4
                (19.05,         math.radians(170)), # p5
                (6.0,           math.radians(80)), # p6
                (19.05,         math.radians(170)), # p7
                (5.0,           math.radians(80)), # p8
        ]
        # The last row has an extra key
        if net_id == net_ids['/row5']:
            movements.append(((19.05 * 2.0) - 1.27,  math.radians(170)))  # p9
        else:
            movements.append((19.05 - 1.27,  math.radians(170)))  # p9
        build_path(start, movements).print_segments("B.Cu", net_id)

def generate_traces(args: argparse.Namespace) -> int:
    bloomer_dir = get_bloomer_repo_dir()
    print("Bloomer repo directory: {}".format(bloomer_dir))

    sd = SwitchData()
    columns(sd)
    switch_to_diodes(sd)
    diode_rows(sd)



    return 0
