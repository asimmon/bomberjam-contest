#!/bin/sh

set -e

javac -cp '.:*' MyBot.java
../../engine/bomberjam --output replay.json "java -cp '.:*' MyBot --logging" "java -cp '.:*' MyBot" "java -cp '.:*' MyBot" "java -cp '.:*' MyBot"