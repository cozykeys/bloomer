#!/usr/bin/env python3

import math

def main():
    theta = math.radians(10.0)
    magnitude = 11.0

    (x0, y0) = (28.100, 411.425)

    print('(x0, y0) = ({}, {})'.format(x0, y0))

    (x1, y1) = (
        round(x0 + (magnitude * math.cos(theta)), 3),
        round(y0 + (magnitude * math.sin(theta)), 3)
    )

    print('(x1, y1) = ({}, {})'.format(x1, y1))

    (x2, y2) = (40.585, 413.335)

    print('(x2, y2) = ({}, {})'.format(x2, y2))

    (dx, dy) = (
        round(x2 - x1, 3),
        round(y2 - y1, 3)
    )

    print('(dx, dy) = ({}, {})'.format(dx, dy))

    return 0

if __name__ == '__main__':
    exit(main())
