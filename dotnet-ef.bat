@echo off

./compose-dev exec backend /entrypoint.dev.sh "~/.dotnet/tools/dotnet-ef --no-build --project /backend/AgileApp %*"