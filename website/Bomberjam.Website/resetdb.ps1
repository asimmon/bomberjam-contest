$ErrorActionPreference = "Stop"

dotnet ef database drop --force
dotnet ef database update