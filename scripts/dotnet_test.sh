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

while read x
do
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
            if [ $errors ]
            then
                errors="$errors\n$x"
            else
                errors="$x"
            fi
        ;;
        # 'Results File:'*)
        #     [[ $x =~ $regexResults ]]
        #     echo "Results in file: ${BASH_REMATCH[1]}"
        # ;;
    esac
    echo $x
#done < <(dotnet test -c Debug -r ~/tests/trx -o ~/bin --logger:trx src/Mellis.all.sln --no-build --no-restore)
done < <(dotnet test --logger:trx --no-build --no-restore "$@")

echo "export TEST_PASSED=$passed" >> $BASH_ENV
echo "export TEST_FAILED=$failed" >> $BASH_ENV
echo "export TEST_SKIPPED=$skipped" >> $BASH_ENV
echo "export TEST_TOTAL=$total" >> $BASH_ENV
echo "export TEST_ERRORS=<<ERROR_STRINGS\n$errors\nERROR_STRINGS" >> $BASH_ENV

echo "<<<<<< Testing complete"
echo "Total passed: $TEST_PASSED"
echo "Total failed: $TEST_FAILED"
echo "Total skipped: $TEST_SKIPPED"
echo "Total num of tests: $TEST_TOTAL"
echo
echo "Found errors:"
echo "$TEST_ERRORS"
echo

exit $TEST_FAILED