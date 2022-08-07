# Bloomer v2 Keyboard Build Guide

![bloomer keyboard](https://djo703t8kjftd.cloudfront.net/images/keyboards/bloomer/bloomer-front_1600x1600.jpg)

## **Materials:**

 - Soldering Iron + Solder
 - 1N4148 Diodes ([example](https://www.mouser.com/ProductDetail/onsemi-Fairchild/1N4148?qs=i4Fj9T/oRm8RMUhj5DeFQg==))
 - Printed PCB
 - Laser cut case (3 pieces)
 - 12mm M2 standoffs
 - M2 screw heads
 - 6mm x 6mm reset button ([example](https://www.digikey.com/en/products/detail/omron-electronics-inc-emc-div/B3F-1000/33150))
 - Key switches of your choosing
 - Key caps ([key caps shown in photo](https://pimpmykeyboard.com/dsa-standard-keysets-sublimated/))
 - (Optional) WS2812b RGB LEDs


## **Steps**:

1) Obtain all the materials listed above. You can use services like https://ponoko.com to print the case pieces and you can use services like https://jlcpcb.com/ to print the PCB. Just upload the respective files to the site and follow the steps to order them.

2) Solder diodes and clip legs off before anything else. Make sure they are all soldered in the same orientation. One side of the diode should have a ring around it and should be facing DOWN. All of the diodes should be on the back side of the PCB (the side with the Bloomer/ Ergo87 text one it) **except for one mentioned in step 3**.

3) One switch and diode are soldered onto the PCB above the microcontroller so **both the switch and diode need to be soldered BEFORE the microcontroller**. This is the only diode that is horizontal and should be the only one on the front side of the PCB, same side that the switch is seated on. The ring side of the diode should be to the left (Q side on a qwerty keyboard). Triple check that this specific diode and switch are soldered correctly because removing a microcontroller to fix it is a nightmare.

4) Solder the reset button. There is an external reset button (part number B3F-1000 Omron tactical switch) located dead center on the back side of the PCB indicated by a (+) icon. This is because the microcontroller is mounted upside down so the built-in reset button is inaccessible. This technically isn't necessary because you can reset the controller by simply touching the RST and GND pins together with a loose wire but that's not fun to do every time you want to adjust your QMK layout. It is highly recommend adding this for easily resetting the firmware.

5) Mount switches into the plate and solder them. It is recommended that you don't try to put all of the switches into the plate before soldering, especially if using PCB mount. Clip one switch into the plate at a time, seat it completely into the PCB, and solder. Rinse repeat. It is recommended to do all the corners first and and from there the order doesn't really matter.

6) Solder the microcontroller pin headers. You will need to clip off the long pin headers to be flush with the microcontroller. (See the Speedo build guide for tips on how to do this:  [https://github.com/cozykeys/speedo/blob/master/build_guide.md#microcontroller](https://github.com/cozykeys/speedo/blob/master/build_guide.md#microcontroller))

7) Optionally solder on WS2812b RGB LEDs in the designated areas on the edge of the PCB. There are a total of 12.

8) Flash QMK firmware and test the keyboard, and fix any keys that don't work due to bad solder joints. To flash the firmware, visit https://config.qmk.fm/#/cozykeys/bloomer/v2/LAYOUT, customize key layout to your liking, click compile, once it is done, download the firmware. Then, use QMK Toolbox to flash the firmware to your keyboard.

9) Close up the case using the printed components and the 12mm M2 brass standoffs and screws like these:
	 - [https://www.ebay.com/itm/293622150636](https://www.ebay.com/itm/293622150636)  
	 - [https://www.ebay.com/itm/174331139173](https://www.ebay.com/itm/174331139173)

Some pictures to clarify steps above:  [https://imgur.com/a/y5eZmY8](https://imgur.com/a/y5eZmY8)

For more information about the Bloomer keyboard, visit https://cozykeys.xyz/keyboards/bloomer.

