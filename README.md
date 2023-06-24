# Requirements:
- docker
- docker compose v2
- git

# Getting started:
1. (Optional- needed only for pushing built images) Login to container repository:
`docker login blayer.mooo.com:5000`.
For credentials ask your local devops hehe
2. Run `start-dev` (windows) or `./start-dev` (unix)
3. Docker will download & build the images
4. You are ready to start developing. By default application is hosted on `localhost:4200`
5. Frontend & backend have hot-reload on by default. If you edit & save any file, the watcher will pick it up and rebuild it straight away

# Tips
This setup is done primary for Unix operating systems. Windows batch commands are only there to remain compatibility and may not work as intended!
If you are running this setup on Windows I highly recommend doing it in WSL2. You have to have virtualization enabled for docker, so enabling WSL2 is a matter of 5 minutes.

# Usable commands(unix):
- `./build` builds docker images
- `./push` pushes images to the registry
- `./start` runs application in production environment
- `./start-dev` runs application in development environment
- `./compose` wrapper to `docker compose` which sets up production environment variables & does other cool stuff
- `./compose-dev` same as `./compose` but instead of production it loads development environment variables
- `./dotnet-ef` dotnet-ef tool used for migrations. You can create migrations outside of docker too. But its nice to have ability to do that in docker. Eg. `./dotnet-ef migrations add InitialMigration --project /backend/AgileApp`
- `./generate-migration-scripts` used to generate `.sql` files from existing .NET migrations

# Usable commands(windows):
- `./build` builds docker images
- `./push` pushes images to the registry
- `./start` runs application in production environment
- `./start-dev` runs application in development environment
- `./compose` wrapper to `docker compose` which sets up production environment variables & does other cool stuff
- `./compose-dev` same as `./compose` but instead of production it loads development environment variables
- `./dotnet-ef` dotnet-ef tool used for migrations. You can create migrations outside of docker too. But its nice to have ability to do that in docker. Eg. `./dotnet-ef migrations add InitialMigration --project /backend/AgileApp`

# Running in WSL
In WSL you may encounter problems with mounting volumes: `rslave is mounted on / but it is not a shared or slave mount`

You have to `sudo mount --make-rshared /` to make it go away, but you have to do this every time you log in.

It is not recommended but if its annoying for you you can add mount command to sudoers:
`user ALL=(ALL) NOPASSWD: /usr/bin/mount`

And then add `sudo mount --make-rshared /` to your .bashrc/.zshrc/other terminal config. Next time you run the project it should run successfully

# Known issues:
- Debbuging backend in Visual Studio. Running the application through Visual Studio should work fine, but backend wont be connected to other services if its run outside of docker environment. So in order do run debugger in Visual Studio you would need to attach to docker container instead of running the application locally, which I have no clue if its possible(good luck hehe).