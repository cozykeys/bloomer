#!/usr/bin/env bash


bloomer_dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
kbutil_dir="$( dirname "$bloomer_dir" )/kbutil"

kbutil_dll="$kbutil_dir/build/KbUtil.Console/bin/Release/kbutil.dll"

input="$bloomer_dir/bloomer.xml"
output="$bloomer_dir/case"

svg_opener="inkscape"

options="--visual-switch-cutouts --keycap-overlays --keycap-legends"

dotnet "$kbutil_dll" gen-svg $options "$input" "$output"
dotnet "$kbutil_dll" gen-key-bearings "$input" "./keys.json" --debug-svg="./temp.svg"

"$svg_opener" "$output/bloomer_Switch.svg" 

