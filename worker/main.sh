#!/bin/sh

set -e

## More information about control groups:
## https://www.digitalocean.com/community/tutorials/how-to-limit-resources-using-cgroups-on-centos-6

## Creating control groups at runtime with privileged docker run access
/usr/sbin/cgconfigparser -l /etc/cgconfig.conf

## Deny outgoing network access to bot users
## Also requires privileged docker run access
iptables -A OUTPUT -m owner --uid-owner bot_0 -j DROP
iptables -A OUTPUT -m owner --uid-owner bot_1 -j DROP
iptables -A OUTPUT -m owner --uid-owner bot_2 -j DROP
iptables -A OUTPUT -m owner --uid-owner bot_3 -j DROP

iptables-save | tee /etc/iptables/rules.v4
ip6tables-save | tee /etc/iptables/rules.v6

sudo -H --preserve-env=API_BASE_URL --preserve-env=API_AUTH -iu worker bash -c "python3 /home/worker/worker.py"