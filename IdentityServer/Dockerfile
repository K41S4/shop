FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# restore
COPY ["IdentityServer/IdentityServer.csproj", "IdentityServer/"]
RUN dotnet restore "IdentityServer/IdentityServer.csproj"

# build and publish
COPY ["IdentityServer", "IdentityServer/"]
RUN dotnet publish "IdentityServer/IdentityServer.csproj" -c Release -o /app /p:UseAppHost=false

# Run stage
FROM base AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]