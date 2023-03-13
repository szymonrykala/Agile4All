@echo off

if "%LOCAL_USER_ID%" == "" set "LOCAL_USER_ID=0"
if "%LOCAL_GROUP_ID%" == "" set "LOCAL_GROUP_ID=0"

if "%ENVIRONMENT%" == "DEVELOPMENT" (
    set "ENV=DEVELOPMENT"
    set "ENV_FILE=env/.env.dev"
) else (
    set "ENV=PRODUCTION"
    set "ENV_FILE=env/.env"
)


for /f "delims== tokens=1,2" %%G in (%ENV_FILE%) do set %%G=%%H
 

set "COMMAND=%1"
for /f "tokens=1,* delims= " %%a in ("%*") do set COMMAND_ARGUMENTS=%%b
set "DOCKER_ARGUMENTS=-p %PROJECT_NAME% -f ./docker-compose.yml"

if "%ENV%" == "DEVELOPMENT" (
    set "DOCKER_ARGUMENTS=%DOCKER_ARGUMENTS% %ARGUMENTS% -f ./docker-compose.dev.yml"
) else (
    set "COMMAND_ARGUMENTS=%COMMAND_ARGUMENTS% --no-build"
)

echo Running %ENV% environment

docker compose %DOCKER_ARGUMENTS% %COMMAND% %COMMAND_ARGUMENTS%