#!/usr/bin/env python

class Vec2():
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def to_string(self):
        return '({0}, {1})'.format(self.x, self.y)

class LineSegment():
    def __init__(self, a, b):
        self.a = a
        self.b = b

    def slope(self):
        return (self.b.y - self.a.y) / (self.b.x - self.a.x)

    def y_offset(self):
        return self.a.y - (self.slope() * self.a.x)

def get_intersection_point(s1, s2):
    x = (s2.y_offset() - s1.y_offset()) / (s1.slope() - s2.slope())
    y = s1.slope() * x + s1.y_offset()
    return Vec2(x, y)



def main():
    calculations = [
        (
            LineSegment(Vec2(19.1000004, -65), Vec2(19, -72)),
            LineSegment(Vec2(20, -73), Vec2(147, -73)),
        )
    ]

    for calc in calculations:
        intersection = get_intersection_point(calc[0], calc[1])
        print('<ControlPoint X="{0}" Y="{1}" />'.format(
            intersection.x, intersection.y))

if __name__ == "__main__":
    main()

