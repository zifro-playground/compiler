
# .NET Core environment
FROM mcr.microsoft.com/dotnet/core/sdk:2.2

# # Taken from mono:slim
# ENV MONO_VERSION 5.18.0.225

# RUN apt-get update \
#   && apt-get install -y --no-install-recommends gnupg dirmngr \
#   && rm -rf /var/lib/apt/lists/* \
#   && export GNUPGHOME="$(mktemp -d)" \
#   && gpg --batch --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
#   && gpg --batch --export --armor 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF > /etc/apt/trusted.gpg.d/mono.gpg.asc \
#   && gpgconf --kill all \
#   && rm -rf "$GNUPGHOME" \
#   && apt-key list | grep Xamarin \
#   && apt-get purge -y --auto-remove gnupg dirmngr

# RUN echo "deb http://download.mono-project.com/repo/debian stable-stretch/snapshots/$MONO_VERSION main" > /etc/apt/sources.list.d/mono-official-stable.list \
#   && apt-get update \
#   && apt-get install -y mono-runtime \
#   && rm -rf /var/lib/apt/lists/* /tmp/*

# # Taken from mono:latest
# RUN apt-get update \
#   && apt-get install -y binutils curl mono-devel ca-certificates-mono fsharp mono-vbnc nuget referenceassemblies-pcl \
#   && rm -rf /var/lib/apt/lists/* /tmp/*

# Perhaps future install the pdb2mdb.exe?
# https://gist.github.com/jbevain/ba23149da8369e4a966f
# https://gist.githubusercontent.com/jbevain/ba23149da8369e4a966f/raw/36b3cdd4dd149ab966bbb48141ef8ee2d37c890f/pdb2mdb.exe

# Install git
RUN apt-get update && \
    # apt-get install mono-devel -y \
    apt-get install git -y

# Dotnet tools
RUN dotnet tool install -g dotnet-ildasm
ENV PATH="$PATH:/root/.dotnet/tools"

# Utility scripts
COPY scripts/generate_metafiles.sh /usr/local/bin/generate_metafiles.sh
COPY scripts/git_login.sh /usr/local/bin/git_login.sh
COPY scripts/move_upm_files.sh /usr/local/bin/move_upm_files.sh
COPY scripts/get_dll_version.sh /usr/local/bin/get_dll_version.sh
COPY scripts/commit_upm.sh /usr/local/bin/commit_upm.sh
COPY scripts/slack_notify_github_deploy.sh /usr/local/bin/slack_notify_github_deploy.sh
