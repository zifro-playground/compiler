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

if [ "$TEST_FAILED" -eq 0 ]
then
    # Success
    curl -X POST -H 'Content-type: application/json' \
    --data " { \
\"attachments\": [ \
    { \
        \"fallback\": \"$CIRCLE_BUILD_URL\", \
        \"title\": \":tada: BUILD COMPLETED SUCCESSFULLY\", \
        \"title_link\": \"$CIRCLE_BUILD_URL\", \
        \"footer\": \"Stage: $CIRCLE_STAGE\", \
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
            } , \
            { \
                \"title\": \"Tests\",\
                \"value\": \"Passed: $TEST_PASSED, Failed: $TEST_FAILED, Skipped: $TEST_SKIPPED\", \
                \"short\": true\
            } \
        ], \
        \"actions\": [ \
            { \
                \"type\": \"button\", \
                \"text\": \"Visit Job #$CIRCLE_BUILD_NUM\", \
                \"url\": \"$CIRCLE_BUILD_URL\", \
                \"style\": \"primary\" \
            }, \
            { \
                \"type\": \"button\", \
                \"text\": \"Visit GitHub commit\", \
                \"url\": \"https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$CIRCLE_SHA1\" \
            } \
        ], \
        \"color\": \"#1CBF43\" ,\
        \"ts\": $(date +%s) \
    } \
] } " $SLACK_WEBHOOK

else
    # Fail
    errors=${TEST_ERROR//\n/\\n}
    errors=${errors//\r/\\r}
    errors=${errors//\"/\\\"}
    errors=${errors//\'/\\\'}

    if [ "$errors" ]
    then

        curl -X POST -H 'Content-type: application/json' \
        --data " { \
    \"attachments\": [ \
        { \
            \"fallback\": \"$CIRCLE_BUILD_URL\", \
            \"title\": \":no_entry_sign: BUILD FAILED\", \
            \"title_link\": \"$CIRCLE_BUILD_URL\", \
            \"footer\": \"Stage: $CIRCLE_STAGE\", \
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
                } , \
                { \
                    \"title\": \"Tests\",\
                    \"value\": \"Passed: $TEST_PASSED, Failed: $TEST_FAILED, Skipped: $TEST_SKIPPED\", \
                    \"short\": true\
                }, \
                { \
                    \"title\": \"Failed tests\",\
                    \"value\": \"```\n$errors\n```\", \
                    \"short\": false \
                } \
            ], \
            \"actions\": [ \
                { \
                    \"type\": \"button\", \
                    \"text\": \"Visit Job #$CIRCLE_BUILD_NUM\", \
                    \"url\": \"$CIRCLE_BUILD_URL\" \
                }, \
                { \
                    \"type\": \"button\", \
                    \"text\": \"Visit GitHub commit\", \
                    \"url\": \"https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$CIRCLE_SHA1\" \
                } \
            ], \
            \"color\": \"#ed5c5c\" ,\
            \"ts\": $(date +%s) \
        } \
    ] } " $SLACK_WEBHOOK

    else

        curl -X POST -H 'Content-type: application/json' \
        --data " { \
    \"attachments\": [ \
        { \
            \"fallback\": \"$CIRCLE_BUILD_URL\", \
            \"title\": \":no_entry_sign: BUILD FAILED\", \
            \"title_link\": \"$CIRCLE_BUILD_URL\", \
            \"footer\": \"Stage: $CIRCLE_STAGE\", \
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
                } , \
                { \
                    \"title\": \"Tests\",\
                    \"value\": \"Passed: $TEST_PASSED, Failed: $TEST_FAILED, Skipped: $TEST_SKIPPED\", \
                    \"short\": true\
                } \
            ], \
            \"actions\": [ \
                { \
                    \"type\": \"button\", \
                    \"text\": \"Visit Job #$CIRCLE_BUILD_NUM\", \
                    \"url\": \"$CIRCLE_BUILD_URL\" \
                }, \
                { \
                    \"type\": \"button\", \
                    \"text\": \"Visit GitHub commit\", \
                    \"url\": \"https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$CIRCLE_SHA1\" \
                } \
            ], \
            \"color\": \"#ed5c5c\" ,\
            \"ts\": $(date +%s) \
        } \
    ] } " $SLACK_WEBHOOK

    fi

fi

echo "Job completed successfully. Alert sent."