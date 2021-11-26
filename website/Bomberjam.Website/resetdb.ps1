$ErrorActionPreference = "Stop"

try {
    Push-Location $PSScriptRoot
    dotnet ef database drop --force
    dotnet ef database update
} finally {
    Pop-Location
}