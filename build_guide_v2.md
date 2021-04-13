# Bloomer Build Guide v2

## Table of Contents

- [Introduction](#introduction)
    - [Vocabulary](#vocabulary)
- [Parts List](#parts-list)
- [Build Steps](#build-steps)
    - [Firmware](#firmware)
    - [Microcontroller Pin Headers](#microcontroller-pin-headers)
    - [Switch Diodes](#switch-diodes)
    - [RGB LEDs](#rgb-leds)
    - [Reset Button](#reset-button)
    - [Switches](#switches)
    - [Microcontroller](#microcontroller)
    - [Testing](#testing)
    - [Case](#case)
- [All Done](#all-done)

## Introduction

This document outlines the steps to build a v2 Bloomer keyboard. Note that the
Bloomer was still called the Ergo87 at that time so there are a few places
where artifacts of that name still exist, such as on the zipped gerber files
and silk screened on the PCB.

### Vocabulary

- [MCU](https://en.wikipedia.org/wiki/Microcontroller): Microcontroller unit,
  in this case the Adafruit ItsyBitsy
- [Via](https://en.wikipedia.org/wiki/Via_(electronics)): A hole on the circuit
  board a through-hole component is soldered to

## Parts List

- Bloomer v2 PCB
    - Not available for sale yet so you'll need to get it manufactured online -- I use [PCBWay](https://www.pcbway.com/)
    - Gerber files are available in the [v2 release folder](./revisions/rev2)
- Bloomer Case
    - Not available for sale yet so you'll need to get it manufactured online -- I use [Ponoko](https://www.ponoko.com)
    - Case files are available in the [v2 release folder](./revisions/rev2)
    - Top and bottom layers should be between 1.5 and 3.0 mm thick
        - For acrylic, I recommend 3.0 as 1.5 is very brittle and liable to crack while inserting switches
    - Middle layers should be add up to at least 12mm thick
        - For example, either 4 x 3.0mm or 2 x 4.5mm and 1 x 3.0mm would work
- Adafruit ItsyBitsy 32u4 5V 16Hz Microcontroller
    - Available on [Adafruit's website](https://www.adafruit.com/product/3677)
- 87 x 1N4148 Diodes; through-hole and SMD are both supported
    - [Through-hole](https://www.digikey.com/en/products/detail/on-semiconductor/1N4148/458603)
    - [SMD](https://www.digikey.com/en/products/detail/taiwan-semiconductor-corporation/1N4148W-RHG/7357066)
- 87 x Switches
    - PCB or plate mount are both supported
    - Only MX-style switches will work; choc low profile, matias, etc will not
    - If using a plate thicker than 1.5mm thick, plate mount will be "friction fit" meaning that they won't clip onto the plate
        - This is perfectly fine but make sure switches are aligned before soldering
- 12 x WS2812b RGB LEDs (Optional)
    - https://www.digikey.com/en/products/detail/adafruit-industries-llc/1655/5154679
- 28 x ~6mm M2 Screws
    - https://www.ebay.com/itm/10-50-M2-M6-SS304-Allen-Hex-Hexagon-Socket-Ultra-Thin-Flat-Wafer-Head-Screw-Bolt/153550101060
- 14 x ~12mm M2 Standoffs
    - https://www.ebay.com/itm/New-highest-quality-2mm-Brass-Standoff-Spacer-M2-Female-x-M2-Female-Freeshipping/182032208680

## Build Steps

### Firmware

It's always a good idea to flash the firmware first in case the MCU is dead. The firmware and instructions are available in the QMK repository:

TODO

### Microcontroller Pin Headers

In the Bloomer, the MCU sits underneath the top-most center key so the order in
which components are soldered here is important. I recommend starting with the
pin headers.

Ensure that the PCB is oriented correctly. The "back" side of the circuit
board, the side in which the pin headers and eventually the MCU should sit, is
the side that has the "Ergo87" text screen printed towards the top. One of the
pin header vias is also marked with **RST**.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_221105_1600x1600.png" alt="MCU Location" width="480" height="480"/>
</p>

Pin headers typically have longer pins on one side. I recommend putting the
shorter pins through the circuit board so that the longer pins are sticking up
for the MCU to sit atop.

Before soldering, I like to put all three segments of pin headers into place,
put the MCU into place, and then apply electrical tape to keep everything lined
up nicely. Then, flip the PCB over and solder the legs to the circuit board. I
typically start with the end pins on each segment so everything is secured,
then move on to the rest of the pins.

**Do not solder the MCU into place yet.** That needs to be done after the
switches.

### Switch Diodes

The next few steps (Up to the MCU section) can be done in any order but if you
just follow the steps in the order recommended, all will be well.

Next to each switch are the vias and pads for the diodes. These appear as a
square hole, two small square pads, and a circircular hole in a straight line.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_221246_1600x1600.png" alt="Diode Vias/Pads" width="480" height="480"/>
</p>

**Polarity matters on diodes.** If using through-hole diodes, align them such
that the black stripe is closer to the circular via and farther from the square
via.

For all of the diodes except for the one above the MCU, the diode should be
on the back side of the PCB. On my builds, for the diode that sits within the
MCU header pins, I chose to put this on the front side of the PCB to avoid any
collision with the MCU. If using a thicker top plate, this diode might need to
be placed on the back side but I haven't tested whether or not this would cause
problems.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_221447_1600x1600.png" alt="Soldered Diode" width="480" height="480"/>
</p>

Similarly, if using SMD diodes, align them such that the line on the diode is
oriented towards the circular pad.

### RGB LEDs

The WS2812b RGB LEDs can be quite frustrating to solder by hand so my best
advice for this portion is to be patient and don't fret if things don't work on
the first try.

The orientation of the LEDs is very important. One corner of the WS2812b will
have a notch in it and the silk screen on the PCB indicates where this corner
should go via an angled corner next to the corresponding pad.

I have a technique that I generally used when soldering SMD components like
these. Begin by heating just one of the pads and applying a small dab of
solder. Next, move the component into place with one hand using tweezers while
using the other hand to apply heat to the solder from the previous step with
the iron. Finally, with the component in place, complete the remaining pads by
applying solder wire and heat as usual.

One thing that makes these more frustrating to solder is that the legs are
under the component and raise it up off of the PCB slightly. While placing it,
I recommend pressing down slightly to ensure it sits as flush as possible with
the PCB.

Also, these LEDs are fairly easy to burn out so try not to hold the iron to a
pad for too long. Instead, touch the iron to the pad and apply the solder wire
briefly. If it doesn't flow correctly, simply pull the iron/wire back, give it
time to cool, and try again.

The final result will look as follows.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_222353_1600x1600.png" alt="RGBs Soldered" width="480" height="480"/>
</p>

**Note:** In more recent keyboard designs I have started adding decoupling
capacitors next to each of the RGBs. I won't get into why this is important but
I've had issues with the RGB LEDs on the v2 Bloomer. I'm not sure if these are
due to the lacking capacitors, crappy soldering jobs, or a bit of both;
however, don't be surpised if these suffer from unstable voltage.

### Reset Button

Since the MCU is soldered upside down, the reset button on the MCU itself is
inaccessible. So, to make flashing firmware easier, there is support for a
tactile reset switch on the back side of the PCB.

This is very simple to solder as the polarity of the button doesn't matter.
Just make sure to place the button on the correct side of the PCB, which is the
side with the silk screen graphics.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_222955_1600x1600.png" alt="Reset Button Location" width="480" height="480"/>
</p>

### Switches

The switches are pretty straight forward. In an order that makes sense, place
each switch into the top plate, line it up with the top side of the PCB -- the
side opposite from where all the diodes/RGBs/reset button were soldered --
ensure the switch is pushed all the way into the PCB, and solder the two
through hole pins.

I like to start in the corners to make sure the PCB is completely lined up,
then I just work inward column by column.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_223537_1600x1600.png" alt="Switches Soldered Top" width="480" height="480"/>
</p>

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_223550_1600x1600.png" alt="Switches Soldered Bottom" width="480" height="480"/>
</p>

Take special care to correctly solder the top middle switch that will sit above
the MCU. If any other switch is improperly soldered, it can easily be fixed at
any time; however, the pins on this one will be inaccessible as soon as the MCU
is attached. As such, it is important to fully inspect the solder joints and
make sure they look good. Desoldering microcontrollers is not fun.

### Microcontroller

With the switches in place, it's time to finish up by attaching the MCU. Place
the MCU onto the pin headers such that the USB port is sandwiched between the
ItsyBitsy's silicon board and the PCB.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_223916_1600x1600.png" alt="MCU Orientation" width="480" height="480"/>
</p>

The pin headers need to be clipped to fit within the case and this is
especially important when the case material is conductive, such as with the
stainless steel plates.

I've found the easiest way to do this is to place a piece of tape on top of the
MCU to "catch" the pins as they are clipped, otherwise they can fly off and
potentially be dangerous.

Also, It can actually hurt a bit to hold the tape directly due to how thick the
pins are and the force when they are clipped so I also like to place a piece of
foam on top. Anything soft and relatively thick (Like a sock) should work.

The end result is that the top side of the MCU should be quite flat. This
ensures there is plenty of clearance in the case. An unintended benefit is that
this makes the pins a bit easier to solder as well.

Solder the first and last pin on each of the pin header segments so that they
are held in place and make sure that the pin headers are sitting flush against
the MCU.

If everything looks good, solder the rest of the pins.

<p align="center">
<img src="http://assets.cozykeys.xyz/guides/bloomer-rev2/20210412_224615_1600x1600.png" alt="MCU End Result" width="480" height="480"/>
</p>

### Testing

Plug the keyboard in and if you haven't yet flashed the firmware, do so now.

Test that all the keys are working. I generally use:

https://www.keyboardtester.com/

Test that the RGBs are working. Cycle through the RGB modes and make sure that
they look correct. On at least a couple of my builds I've had to fix solder
joints which is why I now leave the case open until it's all tested.

Fortunately, given that the RGBs are in sequence it's usually easy to tell
where the problem is if there is one.

For example, if the first four LEDs look correct but the rest flicker, one or
more solder joints on the fifth LED probably needs to be fixed.

Similarly, if one doesn't light up, it's possible the power pin on that LED is
not properly soldered.

### Case

Once the board appears to be fully functional, go ahead and fasten the case.
The case is fastened by placing the brass spacers into the holes in the middle
case layers and then screwing the M2 screws into the spacers from both the top
and bottom.

Note that as you tighten one side it can actually loosen the other so it's not
a bad idea to use two screwdrivers, one to hold one screw in place and the
other to tighten the opposite screw.

## All Done

Don't forget to take some pictures for internet points.

<p align="center">
<img src="https://djo703t8kjftd.cloudfront.net/images/gallery/bloomer-rev2-1/09-11_20-38-43_00_1600x1600.jpg" alt="Bloomer v2 Finished" width="480" height="480"/>
</p>
