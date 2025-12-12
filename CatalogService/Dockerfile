FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# restore
COPY ["Catalog.WebAPI/Catalog.WebAPI.csproj", "Catalog.WebAPI/"]
COPY ["Catalog.Core/Catalog.Core.csproj", "Catalog.Core/"]
COPY ["Catalog.Persistence/Catalog.Persistence.csproj", "Catalog.Persistence/"]
RUN dotnet restore "Catalog.WebAPI/Catalog.WebAPI.csproj"

# build and publish
COPY ["Catalog.WebAPI", "Catalog.WebAPI/"]
COPY ["Catalog.Core", "Catalog.Core/"]
COPY ["Catalog.Persistence", "Catalog.Persistence/"]
RUN dotnet publish "Catalog.WebAPI/Catalog.WebAPI.csproj" -c Release -o /app /p:UseAppHost=false

# Run stage
FROM base AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Catalog.WebAPI.dll"]