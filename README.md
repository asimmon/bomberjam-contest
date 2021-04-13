# Bomberjam

Below are the required steps to run both the website and worker in a local Windows development environment. 

## Website configuration

Check `appsettings.json` for required parameters. They can be set as environment variables too:

* `GitHub__ClientId`: GitHub OAuth app client ID that redirects to https://localhost:5001/signin-github-callback
* `GitHub__ClientSecret`: GitHub OAuth app client secret that redirects to https://localhost:5001/signin-github-callback
* `GitHub__Administrators`: Comma-separated list of GitHub ID administrators
* `GitHub__StarterKitsArtifactsUrl`: An URL where to download the starter kits
* `SecretAuth__Secret`: A secret token that will be used by the worker to communicate with the website API
* `ConnectionStrings__BomberjamContext`: SQL Server database connection string
* `ConnectionStrings__BomberjamStorage`: Azure Storage connection string, used for bots and game replays storage

For local development, use **Azure Storage Emulator** and **SQL Local DB** for lightweight alternatives to Azure Storage and SQL Server.

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

## Worker Docker configuration

Building the image:

```
cd <project root path>
docker build --tag asimmon/bomberjam-worker:latest -f worker.Dockerfile .
```

The worker Docker container requires specific environment variables to communicate with the Bomberjam API.
Also, privileged access is required by iptables and control groups.

Use the included `docker-compose.yml` to start a worker configured to work with a local hosted website.