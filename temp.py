#!/usr/bin/env python3

import re

SCH_PATH_IN = "/home/pewing/src/github/cozykeys/bloomer/pcb/bloomer_v4.sch"
SCH_PATH_OUT = "/home/pewing/src/github/cozykeys/bloomer/pcb/bloomer_v4_2.sch"

def foo():
    lines_in = []
    lines_out = []
    with open(SCH_PATH_IN, 'r') as f:
        lines_in = f.readlines()


    matches = 1
    for line in lines_in:
        m = re.match("L cozy:MX K([0-9]{2})", line)
        if not m == None:
            index = str(int(m.group(1)) - 1).zfill(2)
            lines_out.append(re.sub("K[0-9]{2}", "K{}".format(index), line, count=1))
            continue

        m = re.match("F 0 \"K([0-9]{2})\" H [0-9]+ [0-9]+ [0-9]+  [0-9]+ C CNN", line)
        if not m == None:
            index = str(int(m.group(1)) - 1).zfill(2)
            lines_out.append(re.sub("K[0-9]{2}", "K{}".format(index), line, count=1))
            continue

        m = re.match("L Device:D_Small D([0-9]{2})", line)
        if not m == None:
            index = str(int(m.group(1)) - 1).zfill(2)
            lines_out.append(re.sub("D[0-9]{2}", "D{}".format(index), line, count=1))
            continue

        m = re.match("F [0-9]+ \"D([0-9]{2})\" V [0-9]+ [0-9]+ [0-9]+  [0-9]+ R CNN", line)
        if not m == None:
            index = str(int(m.group(1)) - 1).zfill(2)
            lines_out.append(re.sub("D[0-9]{2}", "D{}".format(index), line, count=1))
            continue

        lines_out.append(line)

    with open(SCH_PATH_OUT, 'w') as f:
        for line in lines_out:
            f.write(line)

def main():
    foo()

if __name__ == '__main__':
    main()

