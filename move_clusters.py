#!/usr/bin/env python3

import math

def baz():
    y0 = 507.423
    y1 = 449.575

def foo():
    # y = ~527.7

    (x0, y0) = (38.011, 528.271)

    x1 = 28.100

    dx = x0 - x1

    dy = dx * math.tan(math.radians(10))
    dy = round(dy, 3)
    print(dy)

    y1 = round(528.271 - dy, 3)
    print(y1)

    print(round(529.050 - y1, 3))

    # Solve for y1

    # y = mx + b

def bar():
    theta = math.radians(10.0)
    magnitude = 11.0

    (x0, y0) = (28.100, 411.425)

    print('(x0, y0) = ({}, {})'.format(x0, y0))

    (x1, y1) = (
        round(x0 + (magnitude * math.cos(theta)), 3),
        round(y0 + (magnitude * math.sin(theta)), 3)
    )

    print('(x1, y1) = ({}, {})'.format(x1, y1))

    (x2, y2) = (45.602, 412.274)

    print('(x2, y2) = ({}, {})'.format(x2, y2))

    (dx, dy) = (
        round(x2 - x1, 3),
        round(y2 - y1, 3)
    )

    print('(dx, dy) = ({}, {})'.format(dx, dy))


def main():
    #bar()
    foo()

    return 0

if __name__ == '__main__':
    exit(main())
