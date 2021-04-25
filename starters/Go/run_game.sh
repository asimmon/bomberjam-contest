#!/bin/sh

set -e
go build main

../../engine/bomberjam --output replay.json "go run main --logging" "go run main" "go run main" "go run main"