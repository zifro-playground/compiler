
# .NET Core environment
FROM mcr.microsoft.com/dotnet/core/sdk:2.2

# Install trx2junit
RUN dotnet tool install -g trx2junit
ENV PATH="$PATH:/root/.dotnet/tools"

# Install git
RUN apt update && \
    apt install git -y

# Utility scripts
COPY scripts/dotnet_test.sh /usr/local/bin/dotnet_test.sh
COPY scripts/slack_notify_testrunner.sh /usr/local/bin/slack_notify_testrunner.sh
