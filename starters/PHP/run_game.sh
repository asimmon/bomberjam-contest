#!/bin/sh

set -e

../../engine/bomberjam --output replay.json "php MyBot.php --logging" "php MyBot.php" "php MyBot.php" "php MyBot.php"