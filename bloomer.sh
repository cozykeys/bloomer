#!/usr/bin/env bash

action="$1"

bloomer_dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

# TODO: Is there a better way? Submodule perhaps?
kbutil_dir="$( dirname "$bloomer_dir" )/kbutil"
kbutil_dll="$kbutil_dir/build/KbUtil.Console/bin/Release/kbutil.dll"

function error() {
    local msg="$1"

    local COLOR_NONE='\033[0m'
    local COLOR_ERROR='\033[0;31m'

	>&2 echo -e "${COLOR_ERROR}ERROR: $msg${COLOR_NONE}"
    exit 1
}

function generate_render() {
    input="$bloomer_dir/bloomer.xml"
    output="$bloomer_dir/case"

    svg_opener="inkscape"

    options="--visual-switch-cutouts --keycap-overlays --keycap-legends"

    dotnet "$kbutil_dll" gen-svg $options "$input" "$output"
    dotnet "$kbutil_dll" gen-key-bearings "$input" "./keys.json" --debug-svg="./temp.svg"

    "$svg_opener" "$output/bloomer_Switch.svg" 
}

function generate_traces() {
    error "Not yet implemented"
}

function print_usage() {
    echo "USAGE: ./bloomer.sh <action> [OPTIONS]"
    echo ""
    echo "Actions:"
    echo "    generate-render   : Generate an svg render of the keyboard"
    echo "    generate-traces   : Generate a kicad_pcb file with keyboard's traces"
}

if [ "$action" = "generate-render" ]; then
    generate_render
elif [ "$action" = "generate-traces" ]; then
    generate_traces
elif [ "$action" = "help" ]; then
    print_usage
else
    print_usage
    exit 1
fi
