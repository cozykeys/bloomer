#!/usr/bin/env python3

#import xml.etree.ElementTree as ET
import lxml.etree as ET
from xml.sax.saxutils import quoteattr

# k01, k02, k03, k04, k05, k06, k07, k08, k09, k10, k11, k12, k13, k14, k15
# k16, k17, k18, k19, k20, k21, k22, k23, k24, k25, k26, k27, k28, k29, k30
# k31, k32, k33, k34, k35, k36, k37, k38, k39, k40, k41, k42, k43, k44, k45
# k46, k47, k48, k49, k50, k51, nil, k52, nil, k53, k54, k55, k56, k57, k58
# k59, k60, k61, k62, k63, k64, k65, k66, k67, k68, k69, k70, k71, k72, k73
# k74, k75, k76, k77, k78, k79, k80, nil, k81, k82, k83, k84, k85, k86, k87

# Y Values
# 0: -47.625
# 1: -28.575
# 2:  -9.525
# 3:   9.525
# 4:  28.575
# 5:  47.625

# Column Offsets

col00_offset = 2.5
col01_offset = 2.5
col02_offset = -0.5
col03_offset = -5.5
col04_offset = 0.5
col05_offset = 5.5
col06_offset = 5.5

col08_offset = 5.5
col09_offset = 5.5
col10_offset = 0.5
col11_offset = -5.5
col12_offset = -0.5
col13_offset = 2.5
col14_offset = 2.5

pos_table = {
    # col00
    'k01': -47.625 + col00_offset,
    'k16': -28.575 + col00_offset,
    'k31':  -9.525 + col00_offset,
    'k46':   9.525 + col00_offset,
    'k59':  28.575 + col00_offset,
    'k74':  47.625 + col00_offset,
    # col01
    'k02': -47.625 + col01_offset,
    'k17': -28.575 + col01_offset,
    'k32':  -9.525 + col01_offset,
    'k47':   9.525 + col01_offset,
    'k60':  28.575 + col01_offset,
    'k75':  47.625 + col01_offset,
    # col02
    'k03': -47.625 + col02_offset,
    'k18': -28.575 + col02_offset,
    'k33':  -9.525 + col02_offset,
    'k48':   9.525 + col02_offset,
    'k61':  28.575 + col02_offset,
    'k76':  47.625 + col02_offset,
    # col03
    'k04': -47.625 + col03_offset,
    'k19': -28.575 + col03_offset,
    'k34':  -9.525 + col03_offset,
    'k49':   9.525 + col03_offset,
    'k62':  28.575 + col03_offset,
    'k77':  47.625 + col03_offset,
    # col04
    'k05': -47.625 + col04_offset,
    'k20': -28.575 + col04_offset,
    'k35':  -9.525 + col04_offset,
    'k50':   9.525 + col04_offset,
    'k63':  28.575 + col04_offset,
    'k78':  47.625 + col04_offset,
    # col05
    'k06': -47.625 + col05_offset,
    'k21': -28.575 + col05_offset,
    'k36':  -9.525 + col05_offset,
    'k51':   9.525 + col05_offset,
    'k64':  28.575 + col05_offset,
    'k79':  47.625 + col05_offset,
    # col06
    'k80':  47.625 + col06_offset,
    # col07
    # col08
    'k81':  47.625 + col08_offset,
    # col09
    'k10': -47.625 + col09_offset,
    'k25': -28.575 + col09_offset,
    'k40':  -9.525 + col09_offset,
    'k53':   9.525 + col09_offset,
    'k68':  28.575 + col09_offset,
    'k82':  47.625 + col09_offset,
    # col10
    'k11': -47.625 + col10_offset,
    'k26': -28.575 + col10_offset,
    'k41':  -9.525 + col10_offset,
    'k54':   9.525 + col10_offset,
    'k69':  28.575 + col10_offset,
    'k83':  47.625 + col10_offset,
    # col11
    'k12': -47.625 + col11_offset,
    'k27': -28.575 + col11_offset,
    'k42':  -9.525 + col11_offset,
    'k55':   9.525 + col11_offset,
    'k70':  28.575 + col11_offset,
    'k84':  47.625 + col11_offset,
    # col12
    'k13': -47.625 + col12_offset,
    'k28': -28.575 + col12_offset,
    'k43':  -9.525 + col12_offset,
    'k56':   9.525 + col12_offset,
    'k71':  28.575 + col12_offset,
    'k85':  47.625 + col12_offset,
    # col13
    'k14': -47.625 + col13_offset,
    'k29': -28.575 + col13_offset,
    'k44':  -9.525 + col13_offset,
    'k57':   9.525 + col13_offset,
    'k72':  28.575 + col13_offset,
    'k86':  47.625 + col13_offset,
    # col14
    'k15': -47.625 + col14_offset,
    'k30': -28.575 + col14_offset,
    'k45':  -9.525 + col14_offset,
    'k58':   9.525 + col14_offset,
    'k73':  28.575 + col14_offset,
    'k87':  47.625 + col14_offset,
}

def process_key(key):
    name = key.attrib["Name"]
    print('Processing key {}'.format(name))
    
    if name in pos_table:
        key.attrib["YOffset"] = str(pos_table[name])


def process_element(element):
    if element.tag == "Key":
        process_key(element)
    else:
        for child in element:
            process_element(child)

def main():
    # Read the file
    root = ET.parse("bloomer.xml").getroot()
    process_element(root)

    et = ET.ElementTree(root)
    et.write('bloomer_test.xml', pretty_print=True)


if __name__ == "__main__":
    main()
