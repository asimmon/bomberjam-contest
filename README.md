# Bomberjam

[HTTPS and a valid certificate](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-5.0) (even self-signed) is required to run the website in a Docker container.
This certificate must be shared from the host to the container using a volume.

## Website Docker configuration

Building the image:

```
cd <project root path>
docker build --no-cache --tag bomberjam-website -f web.Dockerfile .
```

The default localhost development certificate is included in the `.\https` directory.
Running the website with Docker:

```
docker run `
    -e SecretAuth__Secret=verysecret `
    -e ASPNETCORE_URLS="https://+;http://+" `
    -e ASPNETCORE_Kestrel__Certificates__Default__Password=mysuperpassword `
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/localhost.pfx `
    -v <project root path>\https\:/https/ `
    --rm -p 8080:443 bomberjam-website
```

## Worker Docker configuration

Building the image:

```
cd <project root path>
docker build --no-cache --tag bomberjam-worker -f worker.Dockerfile .
```

The worker Docker container requires specific environment variables to communicate with the Bomberjam API.
Also, privileged access is required by iptables and control groups. Running the worker:

```
docker run --rm --privileged `
    -e API_BASE_URL="https://localhost/api/" `
    -e API_AUTH_TOKEN="verysecret" `
    -e API_VERIFY_SSL="0" `
    bomberjam-worker
```