#!/bin/bash

# Set error flags
set -o nounset
set -o errexit
set -o pipefail

dll=${1?Path to DLL required.}

function getVersionBytedString {
    local version_line="$(dotnet-ildasm "${1?}" | grep AssemblyFileVersionAttribute)"
    local regex="00\s(([A-Z0-9]{2}\s?)+)\s00"
    [[ $version_line =~ $regex ]]

    for i in ${BASH_REMATCH[1]/% 00}
    do
        echo -n -e "\u$i"
    done
}

echo $(getVersionBytedString $dll)