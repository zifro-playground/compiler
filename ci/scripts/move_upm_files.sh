#!/bin/bash

# Set error flags
set -o nounset
set -o errexit
set -o pipefail

input=${1?Binaries (input) folder required.}
output=${2?UPM (output) folder required.}

# Remove old files in /Plugins
echo ">>> Remove old files"
find $output/Plugins -not -name '*.meta' -and -not -path '*/\.*' -and -type f |
while read path
do
    rm $path
    echo "Removed $path"
done
echo

# Copy over files
echo ">>> Move Python3 module binaries"
mkdir -p $output/Plugins/se.zifro.mellis.python3
find $input \( -name '*Python3*' -or -name '*Antlr4*' \) -and -not -path '*/\.*' -and -type f \
    -exec mv -v -t $output/Plugins/se.zifro.mellis.python3/ {} +
echo

echo ">>> Move Mellis binaries"
mkdir -p $output/Plugins/se.zifro.mellis
find $input -not -path '*/\.*' -and -type f \
    -exec mv -v -t $output/Plugins/se.zifro.mellis/ {} +
echo
