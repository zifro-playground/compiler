#!/bin/bash

: ${SLACK_WEBHOOK?}

: ${TEST_PASSED?}
: ${TEST_FAILED?}
: ${TEST_SKIPPED?}
: ${TEST_TOTAL?}

: ${TEST_ERRORS:=}
: ${BUILD_STATUS:="fail"}

if [ -z "$SLACK_WEBHOOK" ]; then
    echo "NO SLACK WEBHOOK SET"
    echo "Please input your SLACK_WEBHOOK value either in the settings for this project, or as a parameter for this orb."
    exit 1
fi

function quoteNotFirst {
    local quoteSymbol=""
    while read -r line
    do
        printf "$quoteSymbol%s\n" "$line"
        quoteSymbol="> "
    done <<< "$1"
}

function quote {
    while read -r line
    do
        printf "> %s\n" "$line"
    done <<< "$1"
}

function escape {
    : ${1?}
    local val=${1//\\/\\\\} # \ 
    val=${val//\//\\\/} # / 
    val=${val//\'/\\\'} # ' (not strictly needed ?)
    val=${val//\"/\\\"} # " 
    val=${val//	/\\t} # \t (tab)
    val=${val//
/\\\n} # \n (newline)
    val=${val//^M/\\\r} # \r (carriage return)
    val=${val//^L/\\\f} # \f (form feed)
    val=${val//^H/\\\b} # \b (backspace)
    echo "$val"
}

function getTextForCommit {
    local commitSHA=${1?}
    # local commitSHA="$(git show --pretty=%H --quiet ${1?})"
    local commitShortSHA="$(git show --pretty=%h --quiet $commitSHA)"
    local commitMessage="$(quoteNotFirst "$(git show --quiet --pretty=%B $commitShortSHA)")"

    echo "> <https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$commitSHA|$commitShortSHA> $(escape "$commitMessage")"
}

echo "BUILD_STATUS=$BUILD_STATUS"

if [ "$BUILD_STATUS" == "success" ]
then
    echo "Build successful, adjusing message accordingly"
    # Success
    color="#1CBF43" # green
    title=":tada: BUILD COMPLETED SUCCESSFULLY"
    fallback="Build completed successfully ($CIRCLE_JOB#$CIRCLE_BUILD_NUM)"
    testResults="Passed: $TEST_PASSED :heavy_check_mark:, Failed: $TEST_FAILED, Skipped: $TEST_SKIPPED"
    visitJobActionStyle="primary"
else
    echo "Build failed, adjusing message accordingly"
    # Fail
    color="#ed5c5c" # red
    title=":no_entry_sign: BUILD FAILED"
    fallback="Build failed ($CIRCLE_JOB#$CIRCLE_BUILD_NUM)"
    testResults="Passed: $TEST_PASSED, Failed: $TEST_FAILED :exclamation:, Skipped: $TEST_SKIPPED"
    visitJobActionStyle="danger"

    if [ "$TEST_FAILED" -gt 0 ] && [ "$TEST_ERRORS" ]
    then
        errorsField="*Errors:*\\n\\n$(escape "$TEST_ERRORS")"
    fi
fi

: ${errorsField:=}

commitRange="$CIRCLE_SHA1^...$CIRCLE_SHA1"
echo "Looking at range $commitRange"

if [ "${CIRCLE_API_KEY:-}" ] && [ "${CIRCLE_PREVIOUS_BUILD_NUM:-}" ]
then
    echo "Got CircleCI API key and previous build. Let's find out the SHA1 of last commit..."
    curlResult="$(curl -su $CIRCLE_API_KEY: \
    https://circleci.com/api/v1.1/project/github/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/$CIRCLE_PREVIOUS_BUILD_NUM \
    | grep "\"vcs_revision\" :")"
    # $   "vcs_revision" : "c727a7309ff289d7c38465d7f07d7011658aa4b2",
    curlRegex='vcs_revision.*"(.+)"'
    
    if [[ $curlResult =~ $curlRegex ]] && [[ "${BASH_REMATCH[1]:-}" ]]
    then
        commitPrevSHA=${BASH_REMATCH[1]}
        echo "Found commit '$commitPrevSHA'"
        if [ "$commitPrevSHA" == "$CIRCLE_SHA1" ]
        then
            echo "Oh wait, it's the same commit. Leaving range as-is."
        else
            commitRange="$commitPrevSHA...$CIRCLE_SHA1"
            echo "Instead looking at range $commitRange"
        fi
    else
        echo "No match for previous commit."
    fi
fi

author=""
if [[ $CIRCLE_USERNAME ]]
then
    echo "Looking up commit author $CIRCLE_USERNAME on github..."

    curlResult="$(curl -s https://api.github.com/users/$CIRCLE_USERNAME \
    | grep "\"avatar_url\":")"
    curlRegex='avatar_url.*"(.+)"'

    if [[ $curlResult =~ $curlRegex ]] && [[ "${BASH_REMATCH[1]:-}" ]]
    then
        authorIcon=${BASH_REMATCH[1]}
        echo "Found author profile picture: $authorIcon"

        author="
            \"author_name\": \"$CIRCLE_USERNAME\", \
            \"author_icon\": \"$authorIcon\", \
            \"author_link\": \"https://github.com/$CIRCLE_USERNAME\", \
        "
    fi
fi

text=""
while read commit
do
    echo "Collecting commit: $commit"
    text="$(getTextForCommit $commit)\\n$text"
done < <(git log --pretty=%h $commitRange)

footer="$(git diff --shortstat $commitRange)"
testPercent=$((100*TEST_PASSED/TEST_TOTAL))

curl -X POST -H 'Content-type: application/json' \
--data " { \
\"attachments\": [ \
    { \
        $author \
        \"fallback\": \"$fallback\", \
        \"title\": \"$title\", \
        \"footer\": \"$footer\", \
        \"text\": \"Commits _(oldest first):_\\n$text\\n$errorsField\", \
        \"mrkdwn_in\": [\"fields\", \"text\"], 
        \"color\": \"$color\" ,\
        \"fields\": [ \
            { \
                \"title\": \"Test results: $testPercent %\",\
                \"value\": \"$testResults\", \
                \"short\": true \
            } \
        ], \
        \"actions\": [ \
            { \
                \"style\": \"$visitJobActionStyle\", \
                \"type\": \"button\", \
                \"text\": \"Visit Job #$CIRCLE_BUILD_NUM ($CIRCLE_STAGE)\", \
                \"url\": \"$CIRCLE_BUILD_URL\" \
            } \
        ] \
    } \
] } " $SLACK_WEBHOOK

echo "Job completed successfully. Alert sent."

# Unused project/branch fields

        # \"ts\": $(date +%s) \
    # \"footer\": \"Branch: $CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/$CIRCLE_BRANCH\", \

            # { \
            #     \"title\": \"Project\", \
            #     \"value\": \"$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME\", \
            #     \"short\": true \
            # }, \
            # { \
            #     \"title\": \"Branch\", \
            #     \"value\": \"$CIRCLE_BRANCH\", \
            #     \"short\": true \
            # } \

            
            # { \
            #     \"type\": \"button\", \
            #     \"text\": \"Visit GitHub commit\", \
            #     \"url\": \"https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$commitShortSHA\" \
            # } \