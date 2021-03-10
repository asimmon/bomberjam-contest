#!/bin/sh

if [ -f "package.json" ]; then
    npm ci
fi