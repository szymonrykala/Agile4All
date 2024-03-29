# Requirements:
- docker
- docker compose v2
- git

# Getting started:
1. (Optional- needed only for pushing built images) Login to container repository:
`docker login blayer.mooo.com:5000`.
For credentials ask your local devops hehe
2. Run `./start-dev` (unix)
3. Docker will download & build the images
4. You are ready to start developing. By default application is hosted on `localhost:4200`
5. Frontend & backend have hot-reload on by default. If you edit & save any file, the watcher will pick it up and rebuild it straight away

# Tips
This setup is done primary for Unix operating systems. If you are running this setup on Windows I highly recommend doing it in WSL2. You have to have virtualization enabled for docker, so enabling WSL2 is a matter of 5 minutes. As of writing this docker desktop uses linux containers by default(it uses WSL2 for that), so if you are able to run docker on Windows, then you probably have all its needed there.

# Usable commands(unix):
- `./build` builds docker images
- `./push` pushes images to the registry
- `./start` runs application in production environment
- `./start-dev` runs application in development environment
- `./compose` wrapper to `docker compose` which sets up production environment variables & does other cool stuff
- `./compose-dev` same as `./compose` but instead of production it loads development environment variables
- `./dotnet-ef` dotnet-ef tool used for migrations. You can create migrations outside of docker too. But its nice to have ability to do that in docker. Eg. `./dotnet-ef migrations add InitialMigration --project /backend/AgileApp`
- `./generate-migration-scripts` used to generate `.sql` files from existing .NET migrations

# Env
For development environment default environment variables are used from file `./env/env.dev`. You can override them without triggering git changes by creating `./env/.env` file and placing your altered envs there and then restarting docker containers.
Production uses only `./env/.env`.

# Tests
To run backend tests start the app with `./start-dev` and then after backend container builds successfuly, run `./compose-dev exec backend /entrypoint "dotnet test /backend/AgileApp"`

# Running in WSL
In WSL you may encounter problems with mounting volumes: `rslave is mounted on / but it is not a shared or slave mount`

You have to `sudo mount --make-rshared /` to make it go away, but you have to do this every time you log in.

It is not recommended but if its annoying for you you can add mount command to sudoers:
`user ALL=(ALL) NOPASSWD: /usr/bin/mount`

And then add `sudo mount --make-rshared /` to your .bashrc/.zshrc/other terminal config. Next time you run the project it should run successfully

# Known issues:
- Debbuging backend in Visual Studio. Running the application through Visual Studio should work fine, but backend wont be connected to other services if its run outside of docker environment. So in order do run debugger in Visual Studio you would need to attach to docker container instead of running the application locally, which I have no clue if its possible(good luck hehe).
- If repo is cloned from windows terminal and then run in any unix shell, it might contain invalid line endings. Running bash script will end up with `/bin/bash^M: bad interpreter: No such file or directory` error. It shouldnt happen as its configured to force line endings in .gitattributes file, but in case it does remove the repo and clone it inside unix shell instead.