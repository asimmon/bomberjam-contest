FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["common/Bomberjam.Common/", "common/Bomberjam.Common/"]
COPY ["website/Bomberjam.Website/", "website/Bomberjam.Website/"]
RUN dotnet restore "common/Bomberjam.Common/Bomberjam.Common.csproj"
RUN dotnet restore "website/Bomberjam.Website/Bomberjam.Website.csproj"
WORKDIR "/src/website/Bomberjam.Website"

FROM build AS publish
RUN dotnet publish --no-restore "Bomberjam.Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Bomberjam.Website.dll"]