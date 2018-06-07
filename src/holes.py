#!/usr/bin/env python

from speedo import Point, Square, Circle, Keycap


def new_point(x, y):
    point = Point()
    point.x = x
    point.y = y
    return point


def generate_hole_points():
    original_holes = [
        new_point(15.673, 776.183),
        new_point(473.174, 1132.506),
        new_point(552.796, 1132.506),
        new_point(947.471, 1132.506),
        new_point(1010.301, 776.183),
        new_point(589.778, 607.337),
        new_point(436.194, 607.337)
    ]

    for original_hole in original_holes:
        new_hole = new_point(original_hole.x + 1.736, original_hole.y + 1.736)
        print('{0}\n{1}\n\n'.format(new_hole.x, new_hole.y))


def main():
    generate_hole_points()


if __name__ == "__main__":
    main()
