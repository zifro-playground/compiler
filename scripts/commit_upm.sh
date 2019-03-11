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

git add .
git commit -m ":heavy_check_mark: [CircleCI] Mellis $MELLIS_VERSION, Python3 module $MELLIS_PYTHON3_VERSION
This commit was created autonomously by a script in the CircleCI workflow.

:shipit: $CIRCLE_BUILD_URL
:octocat: https://github.com/$CIRCLE_PROJECT_USERNAME/$CIRCLE_PROJECT_REPONAME/commit/$CIRCLE_SHA1"
# TODO: tag
echo

echo ">>> Pushing to $CIRCLE_REPOSITORY_URL"
if [ -n "${LOCAL:-}" ]; then
    echo "(not pushing because local dev environment)"
else
    git push
fi
