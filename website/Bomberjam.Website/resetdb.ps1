$ErrorActionPreference = "Stop"

Remove-Item .\Migrations\ -Recurse -Force
dotnet ef migrations add Initial
dotnet ef database drop --force
dotnet ef database update