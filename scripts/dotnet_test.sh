#!/bin/bash

# project needs to be pre-compiled
# example usage
# dotnet_test ~/src/mySolution.sln -c Debug -r ~/trx -o ~/bin

set -o nounset
set +o errexit
set +o pipefail

echo ">>>>>> Commence testing"

passed=$((0))
failed=$((0))
skipped=$((0))
total=$((0))

regexTests='Passed: ([0-9]+).*Failed: ([0-9]+).*Skipped: ([0-9]+)'
regexResults='^Results File: (.*)$'

errors=""

function escapeJson {
    : ${1?}
    local val=${1//\\/\\\\} # \ 
    # val=${val//\//\\\/} # / 
    # val=${val//\'/\\\'} # ' (not strictly needed ?)
    val=${val//\"/\\\"} # " 
    val=${val//	/\\t} # \t (tab)
    # val=${val//^M/\\\r} # \r (carriage return)
    val="$(echo "$val" | tr -d '\r')" # \r (carrige return)
    val=${val//
/\\\n} # \n (newline)
    val=${val//^L/\\\f} # \f (form feed)
    val=${val//^H/\\\b} # \b (backspace)
    echo -n "$val"
}

while read x
do
    echo $x
    case "$x" in
        'Total tests:'*)
            [[ $x =~ $regexTests ]]
            ((passed+=${BASH_REMATCH[1]}))
            ((failed+=${BASH_REMATCH[2]}))
            ((skipped+=${BASH_REMATCH[3]}))
            ((total=passed+failed+skipped))
            echo "<<< found $total tests"
        ;;
        'Failed '*)
            if [ "$errors" ]
            then
                errors="$errors\n> :small_red_triangle_down: $x"
            else
                errors="> :small_red_triangle_down: $x"
            fi
            read x # discard "Error Message:" line
            echo $x
            read x # the error message
            echo $x

            errors="$errors\n> \`\`\`\n$(escapeJson "$x")\n\`\`\`"
        ;;
        # 'Results File:'*)
        #     [[ $x =~ $regexResults ]]
        #     echo "Results in file: ${BASH_REMATCH[1]}"
        # ;;
    esac
#done < <(dotnet test -c Debug -r ~/tests/trx -o ~/bin --logger:trx src/Mellis.all.sln --no-build --no-restore)
done < <(dotnet test --logger:trx --no-build --no-restore "$@")

echo "export TEST_PASSED=$(($passed))" >> $BASH_ENV
echo "export TEST_FAILED=$(($failed))" >> $BASH_ENV
echo "export TEST_SKIPPED=$(($skipped))" >> $BASH_ENV
echo "export TEST_TOTAL=$(($total))" >> $BASH_ENV
echo -e "read -rd '' TEST_ERRORS <<'ERROR_STRINGS'\n$errors\nERROR_STRINGS\n" >> $BASH_ENV
echo "export TEST_ERRORS" >> $BASH_ENV

echo "<<<<<< Testing complete"
echo "Total passed: $passed"
echo "Total failed: $failed"
echo "Total skipped: $skipped"
echo "Total num of tests: $total"
echo
echo "Found errors:"
echo -e "$errors"
echo

exit $failed