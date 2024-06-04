# Gebruik een bestaande image als basis
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Kopieer de projectbestanden en restore dependencies
COPY ["Album.Api/Album.Api.csproj", "Album.Api/"]
RUN dotnet restore "Album.Api/Album.Api.csproj"

# Kopieer de rest van de bronbestanden en build de applicatie
COPY . .
WORKDIR "/src/Album.Api"
RUN dotnet build "Album.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Album.Api.csproj" -c Release -o /app/publish

# Stel de werkdirectory in en voer de app uit
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Album.Api.dll"]
