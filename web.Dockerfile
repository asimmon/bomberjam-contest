FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["engine/Bomberjam.Common/", "engine/Bomberjam.Common/"]
COPY ["website/Bomberjam.Website/", "website/Bomberjam.Website/"]
RUN dotnet restore "engine/Bomberjam.Common/Bomberjam.Common.csproj"
RUN dotnet restore "website/Bomberjam.Website/Bomberjam.Website.csproj"
WORKDIR "/src/website/Bomberjam.Website"
# RUN dotnet build "Bomberjam.Website.csproj" -c Debug -o /app/build

FROM build AS publish
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "Bomberjam.Website.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Bomberjam.Website.dll"]