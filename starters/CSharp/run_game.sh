#!/bin/sh

dotnet build

./bomberjam --output replay.json "dotnet bin/Debug/netcoreapp3.1/MyBot.dll" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll"