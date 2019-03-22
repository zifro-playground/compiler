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

function escape {    
local JSON_TOPIC_RAW=${1//\\/\\\\} # \ 
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//\//\\\/} # / 
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//\'/\\\'} # ' (not strictly needed ?)
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//\"/\\\"} # " 
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//	/\\t} # \t (tab)
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//
/\\\n} # \n (newline)
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//^M/\\\r} # \r (carriage return)
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//^L/\\\f} # \f (form feed)
JSON_TOPIC_RAW=${JSON_TOPIC_RAW//^H/\\\b} # \b (backspace)
    printf "%s\n" $JSON_TOPIC_RAW
}

function quote {
    local output=""
    local newline=""
    while read -r line
    do
        # line=${line//$"\""/$"\\\""}
        # line=${line//$"\'"/$"\\'"}
        # line=${line//$"\`"/$"\\\`"}
        printf "%s" $line$newline
        # if [ "$output" ]
        # then
        #     output="$output\\n> ${line}"
        # else
        #     output="> ${line}"
        # fi
        newline=${newline:-"\\n"}
    done
    # printf $output
    printf "\n"
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

curl -X POST -H 'Content-type: application/json' \
--data " { \
\"attachments\": [ \
    { \
        \"fallback\": \"$fallback\", \
        \"title\": \"$title\", \
        \"title_link\": \"$CIRCLE_BUILD_URL\", \
        \"footer\": \"Branch: $CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/$CIRCLE_BRANCH\", \
        \"mrkdwn_in\": [\"fields\"], 
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