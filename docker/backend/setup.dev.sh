#!/bin/bash
apt update && apt install -y wget
wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update && apt install -y dotnet-sdk-6.0