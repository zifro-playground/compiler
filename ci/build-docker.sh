#!/bin/bash

# Set error flags
set -o nounset
set -o errexit

ACCOUNT=${1:-"zifrose"}

echo ">>> Building $ACCOUNT/mellis-upm docker image"
docker build . -t $ACCOUNT/mellis-upm -f mellis-upm.Dockerfile
echo "<<< Successfully built $ACCOUNT/mellis-upm docker image"
echo

echo ">>> Building $ACCOUNT/mellis-testrunner docker image"
docker build . -t $ACCOUNT/mellis-testrunner -f mellis-testrunner.Dockerfile
echo "<<< Successfully built $ACCOUNT/mellis-testrunner docker image"
echo

echo ">>> Uploading $ACCOUNT/mellis-upm docker image"
docker push $ACCOUNT/mellis-upm
echo "<<< Successfully uploaded $ACCOUNT/mellis-upm docker image"
echo

echo ">>> Uploading $ACCOUNT/mellis-testrunner docker image"
docker push $ACCOUNT/mellis-testrunner
echo "<<< Successfully uploaded $ACCOUNT/mellis-testrunner docker image"
echo

echo "Build and push complete"