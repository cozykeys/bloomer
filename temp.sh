#!/usr/bin/env bash

function yell () { >&2 echo "$*";  }
function die () { yell "$*"; exit 1; }
function try () { "$@" || die "Command failed: $*"; }

script_path="$( realpath "$0" )"
script_dir="$( dirname "$script_path" )"

dotnet \
    "/home/pewing/src/github/cozykeys/resources/kbutil/build/KbUtil.Console/bin/Release/kbutil.dll" \
    gen-key-bearings \
    "/home/pewing/src/github/cozykeys/bloomer/bloomer.xml" \
    "./temp/keys.json" \
    --debug-svg="./temp/bearings.svg"


