# CozyKeys Bloomer

This repository contains the design files for the CozyKeys Bloomer, an 87-key
ergonomic mechanical keyboard.

This keyboard has undergone several revisions and name changes. Previous names
were *Atreus87* and *Ergo87* and artifacts of these may still be present in the
repository.

![Bloomer](http://assets.cozykeys.xyz/images/keyboards/bloomer/bloomer-angle-2_800x800.jpg)

## Firmware

- [Bloomer rev2](https://github.com/cozykeys/qmk_firmware/tree/bloomer_v2)
- [Bloomer rev3](https://github.com/cozykeys/qmk_firmware/tree/bloomer_v3)

## Project Status

### rev0

The first revision of the board was hand-wired and built as a proof of concept
to ensure that the layout is comfortable.

### rev1

The second revision added a PCB and a complete redesign of the case so that the
PCB would fit accurately.

This initial PCB was functional but lacked any RGB support. It also had a few
issues:
- When placing the ItsyBitsy upside down to reduce the height needed between
  the switch plate and bottom plate, there is no easy way to access RST
- Switch footprints were "flippable" which was misleading as the controller
  pinout only supported a single orientation

### rev2

The second revision added several features and fixes:
- Dedicated reset button with the case updated to make this accessible
- Single orientation switch footprints
- RGB underglow support

[Rev2 Build Guide](./revisions/rev2/README.md)

### rev3 (REDACTED)

See: [v3.0 README](./v3.0/README.md)
