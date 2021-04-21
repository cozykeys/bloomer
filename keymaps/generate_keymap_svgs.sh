#!/usr/bin/env bash

function yell () { >&2 echo "$*";  }
function die () { yell "$*"; exit 1; }
function try () { "$@" || die "Command failed: $*"; }

script_path="$( realpath "$0" )"
script_dir="$( dirname "$script_path" )"

keymaps_dir="$script_dir"

svg_opener="inkscape"

[ -z "$(which kbutil)" ] && die "ERROR: kbutil not found in \$PATH"

options="--visual-switch-cutouts --keycap-overlays --keycap-legends"

try kbutil gen-svg $options \
    "$keymaps_dir/v2/default/keymap_v2_default.xml" \
    "$keymaps_dir/v2/default"

try kbutil gen-svg $options \
    "$keymaps_dir/v3/default/keymap_v3_default.xml" \
    "$keymaps_dir/v3/default"

"$svg_opener" \
    "$keymaps_dir/v2/default/bloomer_layer_default.svg" \
    "$keymaps_dir/v2/default/bloomer_layer_fn.svg" \
    "$keymaps_dir/v3/default/bloomer_layer_default.svg" \
    "$keymaps_dir/v3/default/bloomer_layer_fn.svg"
