FROM ubuntu:20.04

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1

## Install updates and required tools with disabled prompts
RUN echo iptables-persistent iptables-persistent/autosave_v4 boolean true | debconf-set-selections && \
    echo iptables-persistent iptables-persistent/autosave_v6 boolean true | debconf-set-selections && \
    apt-get update && \
    apt-get -y upgrade && \
    DEBIAN_FRONTEND=noninteractive apt-get -y install wget curl sudo systemd iptables-persistent cgroup-tools apt-transport-https dos2unix software-properties-common

## Python 3.8 with pip, Java 8, .NET Core 3.1, .NET 5.0 & 6.0, Node.js, Go 1.16.3 and PHP 8.0
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    curl -sL https://deb.nodesource.com/setup_12.x | bash - && \
    add-apt-repository ppa:ondrej/php

RUN apt-get update && apt-get install -y python3 python3-pip openjdk-8-jdk dotnet-sdk-3.1 dotnet-sdk-5.0 dotnet-sdk-6.0 nodejs php8.0 && \
    wget -c https://golang.org/dl/go1.16.3.linux-amd64.tar.gz -O - | sudo tar -xz -C /usr/local && \
    echo 'export PATH=$PATH:/usr/local/go/bin' >> /etc/skel/.profile && \
    echo 'export PATH=$PATH:/usr/local/go/bin' >> ~/.profile

## 1. Create four users to isolate bots execution, all network will be disabled for them: bot_0, bot_1, bot_2 and bot_3
## 2. Create a user to be used compilation tasks: bot_compilation
## 3. Create a user to be used by the worker exclusively: worker. This account will be able to sudo as any bot_[0-3] or bot_compilation user
## 4. Disable fqdn in sudo. This prevents most DNS lookups, and hopefully will avoid the `sudo: failed to resolve` error that comes up.
RUN useradd -m -U bot_0 && \
    useradd -m -U bot_1 && \
    useradd -m -U bot_2 && \
    useradd -m -U bot_3 && \
    useradd -m -U bot_compilation && \
    useradd -m -U worker -G bot_0,bot_1,bot_2,bot_3,bot_compilation -s /bin/bash && \
    sh -c "echo \"worker ALL=(bot_compilation) NOPASSWD: ALL\" > /etc/sudoers.d/worker_bot_compilation" && \
    sh -c "echo \"worker ALL=(bot_0) NOPASSWD: ALL\" > /etc/sudoers.d/worker_bot_0" && \
    sh -c "echo \"worker ALL=(bot_1) NOPASSWD: ALL\" > /etc/sudoers.d/worker_bot_1" && \
    sh -c "echo \"worker ALL=(bot_2) NOPASSWD: ALL\" > /etc/sudoers.d/worker_bot_2" && \
    sh -c "echo \"worker ALL=(bot_3) NOPASSWD: ALL\" > /etc/sudoers.d/worker_bot_3" && \
    chmod 0400 /etc/sudoers.d/worker_bot_compilation && \
    chmod 0400 /etc/sudoers.d/worker_bot_0 && \
    chmod 0400 /etc/sudoers.d/worker_bot_1 && \
    chmod 0400 /etc/sudoers.d/worker_bot_2 && \
    chmod 0400 /etc/sudoers.d/worker_bot_3 && \
    echo 'Defaults !fqdn' | sudo tee /etc/sudoers.d/no-fqdn

# Copy bomberjam engine and worker scripts
WORKDIR /home/worker/

COPY ["engine/Bomberjam/", "engine/Bomberjam/"]
COPY ["common/Bomberjam.Common/", "common/Bomberjam.Common/"]
RUN dotnet publish --nologo -c Release -r linux-x64 engine/Bomberjam/Bomberjam.csproj -o . && \
    chmod 0500 bomberjam && \
    chown worker:worker bomberjam && \
    rm -rf engine/ common/

COPY --chown=worker:worker ["worker/*.py", "worker/requirements.txt", "./"]
RUN pip3 install -r requirements.txt && \
    chmod 0400 *.py requirements.txt

WORKDIR /root
COPY ["worker/cgconfig.conf", "/etc/cgconfig.conf"]
COPY ["worker/main.sh", "./"]
RUN chmod 0400 main.sh

CMD /bin/bash /root/main.sh