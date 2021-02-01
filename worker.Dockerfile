FROM ubuntu:20.04

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1

## Install updates and required tools with disabled prompts
RUN echo iptables-persistent iptables-persistent/autosave_v4 boolean true | debconf-set-selections && \
    echo iptables-persistent iptables-persistent/autosave_v6 boolean true | debconf-set-selections && \
    apt-get update && \
    apt-get -y upgrade && \
    DEBIAN_FRONTEND=noninteractive apt-get -y install wget curl sudo systemd iptables-persistent cgroup-tools apt-transport-https

## Python 3.9, Java 8, .NET Core 3.1, .NET Core 5.0, Node.js
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    curl -sL https://deb.nodesource.com/setup_12.x | bash -

RUN apt-get update && apt-get install -y python3 python3.9 python3-pip openjdk-8-jdk dotnet-sdk-3.1 dotnet-sdk-5.0 nodejs

## 1. Create a bots group that will be assigned to all users created below.
## 2. Create a user to be used by the worker exclusively: worker
## 3. Create a user to be used by compilation, and grant sudo access to the worker user: bot_compilation
## 4. Create four users to isolate bots. Deny all network access to them, and also grant sudo access to the worker user: bot_0, bot_1, bot_2 and bot_3
## 5. Disable fqdn in sudo. This prevents most DNS lookups, and hopefully will avoid the `sudo: failed to resolve` error that comes up.
RUN groupadd bots && \
    useradd -m -G bots bot_0 && \
    useradd -m -G bots bot_1 && \
    useradd -m -G bots bot_2 && \
    useradd -m -G bots bot_3 && \
    useradd -m worker -U -G bots,bot_0,bot_1,bot_2,bot_3 -s /bin/bash && \
    useradd -m bot_compilation -G bots && \
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
COPY ["engine/Bomberjam.Common/", "engine/Bomberjam.Common/"]
RUN dotnet publish --nologo -c Release engine/Bomberjam/Bomberjam.csproj -o . && \
    chmod 0500 bomberjam && \
    chown worker:worker bomberjam && \
    rm -rf engine/ && ls -la

COPY --chown=worker:worker ["worker/*.py", "worker/requirements.txt", "./"]
RUN pip3 install -r requirements.txt && \
    chmod 0400 *.py requirements.txt

WORKDIR /root
COPY ["worker/cgconfig.conf", "/etc/cgconfig.conf"]
COPY ["worker/main.sh", "./"]
RUN chmod 0400 main.sh && \
    # to delete
    cp /home/worker/*.py /home/worker/requirements.txt /home/worker/bomberjam . && ls -la .

CMD /bin/bash /root/main.sh