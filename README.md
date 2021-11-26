# Bomberjam

> Bomberjam is now live 🎉: https://bomberjam.anthonysimmon.com

Bomberjam is an **online artificial intelligence programming challenge**.
Write your bot using one of six languages (C#, Go, Java, JavaScript, Python and PHP) and fight against other players to reach the first place in the leaderboard.

Players control a bot using the programming language of their choice. Four bots fight in a 2D grid. The bot with the highest score wins. One player can get points by destroying blocks, hitting another player and being the last player alive.

* Sign in with your GitHub account
* Download  the latest starter kit on GitHub
* Learn the game mechanics and write your bot
* Upload your bot source code as a single zip file
* We will compile your bot and tell you if something went wrong
* We will periodically schedule games and update your score

## Local development setup

### Website configuration

Check `appsettings.json` for required parameters. They can be set as environment variables too:

* `GitHub__ClientId`: GitHub OAuth app client ID that redirects to https://localhost:5001/signin-github-callback
* `GitHub__ClientSecret`: GitHub OAuth app client secret that redirects to https://localhost:5001/signin-github-callback
* `GitHub__Administrators`: Comma-separated list of GitHub ID administrators
* `SecretAuth__Secret`: A secret token that will be used by the worker to communicate with the website API
* `ConnectionStrings__BomberjamContext`: SQL Server database connection string
* `ConnectionStrings__BomberjamStorage`: Azure Storage connection string, used for bots and game replays storage

Use **Azure Storage Emulator** and **SQL Local DB** for lightweight alternatives to Azure Storage and SQL Server.

#### SQL Local DB

Creating the database with initial data is a one time thing:

```
cd <checkoutDirectory>\website\Bomberjam.Website
SqlLocalDB.exe create bomberjam
SqlLocalDB.exe start bomberjam
dotnet ef database update
```

Once you've done that, just start the database when you need it: `SqlLocalDB.exe start bomberjam`


#### Azure Storage Emulator

Just start the emulator and use `UseDevelopmentStorage=true` as connection string.

### Worker Docker configuration

Building the image:

```
cd <project root path>
docker build --tag asimmon/bomberjam-worker:latest -f worker.Dockerfile .
```

The worker Docker container requires specific environment variables to communicate with the Bomberjam API.
Also, privileged access is required by iptables and control groups.

Use the included `docker-compose.yml` to start a worker configured to work with a local hosted website.
