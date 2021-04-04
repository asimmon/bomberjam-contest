#!/bin/sh

dotnet build

../../engine/bomberjam --output replay.json "dotnet bin/Debug/netcoreapp3.1/MyBot.dll --logging" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll" "dotnet bin/Debug/netcoreapp3.1/MyBot.dll"