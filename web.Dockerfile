FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

WORKDIR /src
COPY ["website/Bomberjam.Website/", "Bomberjam.Website/"]
RUN dotnet restore "Bomberjam.Website/Bomberjam.Website.csproj"
WORKDIR "/src/Bomberjam.Website"
RUN dotnet build "Bomberjam.Website.csproj" -c Debug -o /app/build

FROM build AS publish
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "Bomberjam.Website.csproj" -c Debug -o /app/publish
RUN dotnet tool install --global dotnet-ef && \
    dotnet ef database drop -f && \
    dotnet ef database update && \
    cp bomberjam.db /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Bomberjam.Website.dll"]