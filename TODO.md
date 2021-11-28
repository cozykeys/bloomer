# Bloomer v4.0 TODO List

- [ ] Add LEDs and OLED back to schematic (oof)
- [ ] Fix up USB-C situation (Errors in pcbnew)
    - Consider just switching to micro usb?
- [ ] Set up edge cuts in kicad plugin
- [ ] Continue on traces


Parts:
- USB-C Connector:
    - https://lcsc.com/product-detail/USB-Type-C_Korean-Hroparts-Elec-TYPE-C-31-M-12_C165948.html
    - See this for tip on drag soldering:
        - https://keeb.io/products/usb-c-port-12-pin-hro-type-c-31-m-12

## How to Use USB Type C

(From https://www.reddit.com/r/MechanicalKeyboards/comments/e56wg4/comment/f9i4er8)

For a USB C port you still need the two 22Ω resistors on the D-/D+ lines just
like [this pic from the guide](https://camo.githubusercontent.com/f41b161c63e5e2c3858d364da42bd73db9501a50/68747470733a2f2f7075752e73682f746c4a6e662f616332633336333036642e706e67).

For the USB-C port to work correctly with C-C cables you also need two 5.1k pull down resistors on the CC pins of the port. [Here's a pic I stole from the internet](https://kicad-info.s3.dualstack.us-west-2.amazonaws.com/original/3X/2/6/2658c42dacf38c694f39a472971a274ff3ef5a91.png). This lets the other USB-C port know that your device needs power and allows the other port to detect cable orientation.

Also see:
https://electronics.stackexchange.com/questions/588126/are-usb-type-c-pull-resistor-neccesary-on-cc-and-sbu-pins
