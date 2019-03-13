
# Generate GPG key

```sh
$ gpg --full-generate-key
# follow generate instructions, but no passphrase
# use following settings:
#   kind: (1) RSA and RSA (default)
#   keysize: 4096
#   expiration: /up to you, ex: 1y/
#   real name: /your full name, first+last name/
#   email: /your github.com email/
#   comment: /must be first 3 words of shakespeare's 15th piece/

$ gpg --list-keys --keyid-format LONG
# to find the key id

$ gpg --armor --export $KEY_ID | clip
# paste gpg key at https://github.com/settings/keys

$ gpg --armor --export-secret-keys $KEY_ID | base64 | clip
# paste into GITHUB_GPG_SEC_B64 env var in circleci

# paste KEY_ID into GITHUB_GPG_ID
```

<!-- 
# Generate SSH key

```sh
$ ssh-keygen -t rsa -b 4096 -C "your.github.email@example.com"
# no passphrase, just press enter
$ clip < ~/.ssh/id_rsa.pub
# paste ssh key at https://github.com/settings/keys

$ base64 ~/.ssh/id_rsa | clip
# paste into GITHUB_SSH_KEY_B64 env var in circleci
```
-->

# Other data into CircleCI

Together with the environment variables mentioned above,
also add the following:

Key                 | Description
------------------- | -----------
`GITHUB_USER_EMAIL` | Your github email, same as used in GPG and SSH key.
`GITHUB_USER_NAME`  | Your github display name (not username).

# Import commands inside CircleCI container

These are the steps to be taken in the builder container.

```sh
# Set error flags
set -o nounset
set -o errexit
set -o pipefail

# Load GPG key
GITHUB_GPG_KEY=$(base64 -di - <<< "$GITHUB_GPG_KEY_B64")
gpg --import - <<< "$GITHUB_GPG_KEY"

# # Load SSH key
# GITHUB_SSH_KEY=$(base64 -di - <<< "$GITHUB_SSH_KEY_B64")
# eval $(ssh-agent -s)
# ssh-add - <<< "$GITHUB_SSH_KEY"

# Git settings
git config --global user.name $GITHUB_USER_NAME
git config --global user.email $GITHUB_USER_EMAIL
git config --global user.signingKey $GITHUB_GPG_KEYID
git config --global commit.gpgSign true
git config --global tag.forceSignAnnotated true
```

# Summary: All env vars

List of all environment variables, to check if one is missing in CircleCI

Key                  | Description
-------------------- | -----------
`GITHUB_GPG_ID`      | ID of GPG key.
`GITHUB_GPG_SEC_B64` | GPG private key, base64 encoded.
`GITHUB_USER_EMAIL`  | Your github email, same as used in GPG and SSH key.
`GITHUB_USER_NAME`   | Your github display name (not username).

<!-- `GITHUB_SSH_KEY_B64` | SSH private key, base64 encoded. -->