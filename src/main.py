#!/usr/bin/env python

from speedo import Point, Square, Circle, Keycap, Switch, Column, Line, Path

import xml.etree.ElementTree as ET
import re

def parse_point(point_string):
    tokens = point_string.split(',')
    x = float(tokens[0])
    y = float(tokens[1])
    return Point(x, y)

def parse_path_data(path_data):
    tokens = path_data.split(' ')

    a = parse_point(tokens[1])
    b = parse_point(tokens[2])

    return Line(a, b)

def process_group(group):
    print('Processing group with id \"{0}\"'.format(group.attrib['id']))

    path_elements = []
    for child in group:
        m = re.search(r'^\{.*\}path$', child.tag)
        if not m is None:
            path_elements.append(child)

    print('Found {0} paths'.format(len(path_elements)))
    for path_element in path_elements:
        path = parse_path_data(path_element.attrib['d'])
        print(path.to_string())

def process_layer(layer):
    print('Processing layer with id \"{0}\"'.format(layer.attrib['id']))

    groups = []
    for child in layer:
        m = re.search(r'^\{.*\}g$', child.tag)
        if not m is None:
            groups.append(child)

    print('Found {0} groups'.format(len(groups)))
    for group in groups:
        pass
        process_group(group)


def main():
    tree = ET.parse('../ergomod_switch_plate_ungrouped.svg')
    root = tree.getroot()

    layers = []
    for child in root:
        m = re.search(r'^\{.*\}g$', child.tag)
        if not m is None:
            layers.append(child)

    print('Found {0} layers'.format(len(layers)))
    for layer in layers:
        process_layer(layer)


if __name__ == "__main__":
    main()
