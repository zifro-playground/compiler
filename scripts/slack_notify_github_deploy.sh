#!/bin/bash

: ${SLACK_WEBHOOK?}

: ${MELLIS_VERSION?}
: ${MELLIS_PYTHON3_VERSION?}

: ${DEPLOY_STATUS:="fail"}
: ${DEPLOY_TAG_MELLIS:=}
: ${DEPLOY_TAG_PYTHON3:=}
: ${DEPLOY_TAG_COMBINED:=}
: ${DEPLOY_CHANGESET:=}
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

function escapeJson {
    : ${1?}
    local val=${1//\\/\\\\} # \ 
    val=${val//\//\\\/} # / 
    # val=${val//\'/\\\'} # ' (not strictly needed ?)
    val=${val//\"/\\\"} # " 
    val=${val//	/\\t} # \t (tab)
    # val=${val//^M/\\\r} # \r (carriage return)
    val="$(echo "$val" | tr -d '\r')"
    val=${val//
/\\\n} # \n (newline)
    val=${val//^L/\\\f} # \f (form feed)
    val=${val//^H/\\\b} # \b (backspace)
    echo -n "$val"
}

function getAuthorFields {
    if [[ "${GITHUB_USER_ID:-}" ]]
    then
        echo "Looking up commit author $GITHUB_USER_ID on github..."

        curlResult="$(curl -s https://api.github.com/users/$GITHUB_USER_ID \
        | grep "\"avatar_url\":")"
        curlRegex='avatar_url.*"(.+)"'

        if [[ $curlResult =~ $curlRegex ]] && [[ "${BASH_REMATCH[1]:-}" ]]
        then
            authorIcon=${BASH_REMATCH[1]}
            echo "Found author profile picture: $authorIcon"

            echo "
            \"author_name\": \"deployed by $GITHUB_USER_ID\",
            \"author_icon\": \"$authorIcon\",
            \"author_link\": \"https://github.com/$GITHUB_USER_ID\",
            "
        else
            echo "No profile picture found."
            echo "
            \"author_name\": \"deployed by $GITHUB_USER_ID\",
            \"author_link\": \"https://github.com/$GITHUB_USER_ID\",
            "
        fi
    fi
}

author=""
text="*Project: `$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME` Branch: `$CIRCLE_BRANCH`*"
fields=""
actions=""

if [[ "$BUILD_STATUS" == "success" ]]
then
    if [[ "$DEPLOY_STATUS" == "success" ]]
    then
        # Deploy success
        color="#1CBF43" # green
        visitJobActionStyle="primary" # green
        author="$(getAuthorFields)"
        title=":tada: DEPLOYED TO GITHUB"
        actions=",
            {
                \"type\": \"button\",
                \"text\": \"Visit release $DEPLOY_TAG_COMBINED\",
                \"url\": \"https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/releases/tag/$DEPLOY_TAG_COMBINED\"
            }
        "
        if [[ "$DEPLOY_TAG_MELLIS" ]]; then
            mellisTagText="(tag: <https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/releases/tag/$DEPLOY_TAG_MELLIS|$DEPLOY_TAG_MELLIS>)"
        else
            mellisTagText="(no new tag)"
        fi
        if [[ "$DEPLOY_TAG_PYTHON3" ]]; then
            python3TagText="(tag: <https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/releases/tag/$DEPLOY_TAG_PYTHON3|$DEPLOY_TAG_PYTHON3>)"
        else
            python3TagText="(no new tag)"
        fi
        fields="{
                \"title\": \"Mellis\",
                \"value\": \"```$MELLIS_VERSION```\\n$mellisTagText\",
                \"short\": true
            },
            {
                \"title\": \"Python3 module\",
                \"value\": \"```$MELLIS_PYTHON3_VERSION```\\n$python3TagText\",
                \"short\": true
            }"
    elif [[ "$DEPLOY_STATUS" == "idle" ]]
    then
        # Nothing to deploy
        color="#3AA3E3" # blue
        visitJobActionStyle="default" # gray
        title="NOTHING TO DEPLOY"
        text="$text\\n_No new tags_"
    fi
else
    color="#ed5c5c" # red
    visitJobActionStyle="danger" # red
    title=":no_entry_sign: DEPLOYMENT JOB FAILED"
    text="$text\\n_No new tags_"
fi

footer="$DEPLOY_CHANGESET"
data=" {
\"attachments\": [
    {
        $author
        \"fallback\": \"$fallback\",
        \"title\": \"$title\",
        \"footer\": \"$footer\",
        \"text\": \"$text\",
        \"mrkdwn_in\": [\"fields\", \"text\"], 
        \"color\": \"$color\",
        \"fields\": [
            $fields
        ],
        \"actions\": [
            {
                \"style\": \"$visitJobActionStyle\",
                \"type\": \"button\",
                \"text\": \"Visit Job #$CIRCLE_BUILD_NUM ($CIRCLE_STAGE)\",
                \"url\": \"$CIRCLE_BUILD_URL\"
            }
            $actions
        ]
    }
] }"

echo
response="$(curl -X POST -H 'Content-type: application/json' --data "$data" $SLACK_WEBHOOK)"
if [[ "$response" == "ok" ]]
then
    echo "Job completed successfully. Alert sent."
else
    echo "Something went wrong in the webhook..."
    echo "Payload:"
    echo
    echo "$data"
fi