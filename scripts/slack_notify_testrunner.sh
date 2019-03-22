#!/bin/bash

: ${SLACK_WEBHOOK?}

: ${TEST_PASSED?}
: ${TEST_FAILED?}
: ${TEST_SKIPPED?}
: ${TEST_TOTAL?}

: ${TEST_ERROR:-}

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

if [ "$TEST_FAILED" -eq 0 ]
then
    # Success
    color="#1CBF43" # green$m
    title=":tada: BUILD COMPLETED SUCCESSFULLY"
    fallback="Build completed successfully ($CIRCLE_JOB#$CIRCLE_BUILD_NUM)"

else
    # Fail
    errors=${TEST_ERROR//\n/\\n}
    errors=${errors//\"/\\\"}
    errors=${errors//\'/\\\'}
    errors=${errors//\`/\\\`}

    errorsField=", \
    { \
        \"title\": \"Failed tests\",\
        \"value\": \"\`\`\`\\n$errors\\n\`\`\`\", \
        \"short\": false \
    }"

    color="#ed5c5c" # red
    title=":no_entry_sign: BUILD FAILED"
    fallback="Build failed ($CIRCLE_JOB#$CIRCLE_BUILD_NUM)"
fi

: ${errorsField:=}

cd $CIRCLE_WORKING_DIRECTORY
commitMessage="$(quoteNotFirst "$(git log --pretty=%B -n 1)")"

commitMessage=${commitMessage//\\/\\\\} # \ 
commitMessage=${commitMessage//\//\\\/} # / 
commitMessage=${commitMessage//\'/\\\'} # ' (not strictly needed ?)
commitMessage=${commitMessage//\"/\\\"} # " 
commitMessage=${commitMessage//	/\\t} # \t (tab)
commitMessage=${commitMessage//
/\\\n} # \n (newline)
commitMessage=${commitMessage//^M/\\\r} # \r (carriage return)
commitMessage=${commitMessage//^L/\\\f} # \f (form feed)
commitMessage=${commitMessage//^H/\\\b} # \b (backspace)

commitShortSHA="$(git log --pretty=%h -n 1)"
text="> <https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$commitShortSHA|$commitShortSHA> $commitMessage"

curl -X POST -H 'Content-type: application/json' \
--data " { \
\"attachments\": [ \
    { \
        \"fallback\": \"$fallback\", \
        \"title\": \"$title\", \
        \"title_link\": \"$CIRCLE_BUILD_URL\", \
        \"footer\": \"Branch: $CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/$CIRCLE_BRANCH\", \
        \"text\": \"$text\", \
        \"mrkdwn_in\": [\"fields\", \"text\"], 
        \"fields\": [ \
            { \
                \"title\": \"Project\", \
                \"value\": \"$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME\", \
                \"short\": true \
            }, \
            { \
                \"title\": \"Branch\", \
                \"value\": \"$CIRCLE_BRANCH\", \
                \"short\": true \
            } \
            $errorsField\
        ], \
        \"actions\": [ \
            { \
                \"type\": \"button\", \
                \"text\": \"Visit Job #$CIRCLE_BUILD_NUM ($CIRCLE_STAGE)\", \
                \"url\": \"$CIRCLE_BUILD_URL\" \
            } \
        ], \
        \"color\": \"$color\" ,\
        \"ts\": $(date +%s) \
    } \
] } " $SLACK_WEBHOOK

echo "Job completed successfully. Alert sent."