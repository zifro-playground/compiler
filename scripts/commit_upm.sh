#!/bin/bash

# Set error flags
set -o nounset
set -o errexit
set -o pipefail

repo=${1?Local repo folder required.}
: ${MELLIS_VERSION?}
: ${MELLIS_PYTHON3_VERSION?}
: ${CIRCLE_REPOSITORY_URL?}
: ${CIRCLE_BUILD_URL?}
: ${CIRCLE_PROJECT_USERNAME?}
: ${CIRCLE_PROJECT_REPONAME?}
: ${CIRCLE_SHA1?}

# Working directory
cd $repo

# Commit
echo ">>> Committing changes"

if [ -n "$(git config --get commit.gpgSign)" ]
then
    echo "(Signing commit using key $(git config --get user.signingKey))"
fi

git add . -v
echo
set +e
git commit -m ":heavy_check_mark: [CircleCI] Mellis $MELLIS_VERSION, Python3 module $MELLIS_PYTHON3_VERSION
This commit was created autonomously by a script in the CircleCI workflow.

:shipit: $CIRCLE_BUILD_URL
:octocat: https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$CIRCLE_SHA1"
COMMIT_STATUS=$?
set -e

if [ $COMMIT_STATUS -eq 1 ]
then
    # Nothing to commit.
    echo "<<< Nothing to commit."
elif [ $COMMIT_STATUS -ne 0 ]
then
    echo "<<< Unexpected error during commit. Aborting."
    exit 1
else
    echo ">>> Commit summary"
    git --no-pager show --show-signature --name-status
fi
echo

echo ">>> Tagging"
TAG="m$MELLIS_VERSION-p$MELLIS_PYTHON3_VERSION"
echo "Using tag \"$TAG\""
if [ "$(git tag -l "$TAG")" ]
then
    # Tag duplication.
    echo "<<< Tag already existed. Continuing without tag"
else
    set +e
    git tag "$TAG" -m "Mellis $MELLIS_VERSION, Python3 module $MELLIS_PYTHON3_VERSION"
    TAG_STATUS=$?
    set -e
    if [ $TAG_STATUS -ne 0 ]
    then
        echo "<<< Unexpected error during tagging. Aborting."
        exit 1
    else
        echo ">>> Tag summary"
        git --no-pager show $(git describe --tags) --show-signature --name-status
    fi
fi
echo

echo ">>> Pushing to $CIRCLE_REPOSITORY_URL"
if [ -n "${LOCAL:-}" ]; then
    echo "(not pushing because local dev environment)"
else
    git push --follow-tags
fi
