# Atreus87

This repository contains the design files for the next DIY ergonomic keyboard I
am designing and building. The keyboard is based closely on the Atreus and the
Atreus62; however, it will be fitted with exactly 87 keys, making it a drop-in
replacement for standard TKL keyboards.

The image below shows a preview of the layout of the board.

![Atreus87 Layout Preview][preview]

[preview]: https://github.com/atreus87/atreus87.github.io/blob/master/images/Atreus87.png?raw=true "Atreus87 Layout Preview"

## Generating SVGs

Rather than edit SVG files manually which is a pain for several reasons, the
Atreus87 layout is saved in an XML file. The KbUtil application that resides in
this repository can be used to generate an SVG from that XML file.

To use that tool, you will need the .NET Core SDK version 2.0+.
```bash
dotnet build -c Release src/KbUtil/KbUtil.sln
dotnet src/KbUtil/KbUtil.Console/bin/Release/netcoreapp2.0/kbutil.dll gen-svg Atreus87.xml Atreus87.svg
```

There are a few benefits to this approach discussed below.

### Keycap Overlays
It can be hard to tell just from the switch cutouts whether or not keys are too
close together. By specifying the `--keycap-overlays` command line option when
running the tool, the resulting SVG will contain additional paths outlining
what the keycaps will look like on the keyboard

### Visual Styles
Ponoko expects all the lines to adhere to a specific style where the stroke
width is 0.01mm wide. This makes the lines very difficult to see, so by
specifying the `--visual-switch-cutouts` command line option, all paths will be
drawn with a more visual style.

### Keycap Legends
Keycap legends don't need to be in the SVG when it is sent to a laser cutting
service; however, they can be useful during layout development. So, the
`--keycap-legends` command line option will enable legends being written to the
resulting SVG.
gen-svg --keycap-overlays --visual-switch-cutouts --keycap-legends ../../../../../../Atreus87.xml
