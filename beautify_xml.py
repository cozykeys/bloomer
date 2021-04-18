#!/usr/bin/env python3

import sys

#import xml.etree.ElementTree as ET
import lxml.etree as ET
from xml.sax.saxutils import quoteattr

INDENT = "    "


def attr_string(attr_key, attr_val, indent):
    return "{}{}={}".format(indent + INDENT, attr_key, quoteattr(attr_val))


def print_element(element, indent, attr_str):
    if attr_str:
        print("{}<{}\n{}>".format(indent, element.tag, attr_str))
    else:
        print("{}<{}>".format(indent, element.tag))

    for child in element:
        process_element(child, indent + INDENT)
    print("{}</{}>".format(indent, element.tag))


def print_element_shorthand(element, indent, attr_str):
    if attr_str:
        print("{}<{}\n{} />".format(indent, element.tag, attr_str))
    else:
        print("{}<{} />".format(indent, element.tag))


def is_comment(element):
    return isinstance(element, ET._Comment)

def print_comment(element, indent):
    print("{}<!--{}-->".format(indent, element.text))

def process_element(element, indent):
    if is_comment(element):
        print_comment(element, indent)
        return

    attr_str = (
        None
        if len(element.attrib) == 0
        else "\n".join(
            [
                attr_string(attr_key, element.attrib[attr_key], indent)
                for attr_key in element.attrib
            ]
        )
    )

    # If there are no child elements, use short-hand (I.E. "<tag />" vs. "<tag></tag>")
    if len(element) == 0:
        print_element_shorthand(element, indent, attr_str)
    else:
        print_element(element, indent, attr_str)

def main():
    # Read the file
    root = ET.parse("bloomer.xml").getroot()

    # Read from stdin
    #root = ET.fromstring('\n'.join([l for l in sys.stdin]))

    # There isn't a way to print this using the standard xml.etree.ElementTree
    # library so just hard code it. See:
    # https://stackoverflow.com/questions/15356641/how-to-write-xml-declaration-using-xml-etree-elementtree
    print("<?xml version='1.0' encoding='UTF-8' ?>")

    process_element(root, "")


if __name__ == "__main__":
    main()
