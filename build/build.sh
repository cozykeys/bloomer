#!/usr/bin/env bash

src_dir="../src"

sln_file="$src_dir/KbUtil/KbUtil.sln"


win_runtime="win7-x64"
linux_runtime="linux-x64"

dotnet build -c Release "$sln_file"

dotnet publish \
    --runtime "$win_runtime" \
    --self-contained \
    --configuration Release \
    "$sln_file"

dotnet publish \
    --runtime "$linux_runtime" \
    --self-contained \
    --configuration Release \
    "$sln_file"

