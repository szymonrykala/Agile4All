#!/bin/bash

installed() {
    return $(dpkg-query -W -f '${Status}\n' "${1}" 2>&1|awk '/ok installed/{print 0;exit}{print 1}')
}

if ! installed wget; then
    echo "Package wget not found. Installing..."
    apt-get install -y wget
fi

if ! installed packages-microsoft-prod; then
    echo "Package packages-microsoft-prod not found. Installing..."
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
fi

if ! installed dotnet-sdk-6.0; then
    echo "Package dotnet-sdk-6.0 not found. Installing..."
    apt-get update && apt-get install -y dotnet-sdk-6.0
fi

/entrypoint.sh dotnet tool install --global dotnet-ef --version 6.* &> /dev/null

# Temporary fix until proper prod/dev setup
chown -R user /backend

/entrypoint.sh "$@"