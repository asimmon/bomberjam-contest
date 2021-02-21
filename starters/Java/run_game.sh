#!/bin/sh

set -e

javac -cp '.:*' MyBot.java
./bomberjam --output replay.json "java -cp '.:*' MyBot" "java -cp '.:*' MyBot" "java -cp '.:*' MyBot" "java -cp '.:*' MyBot"