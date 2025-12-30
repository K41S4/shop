FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# restore
COPY ["APIGateway/APIGateway.csproj", "APIGateway/"]
RUN dotnet restore "APIGateway/APIGateway.csproj"

# build and publish
COPY ["APIGateway", "APIGateway/"]
RUN dotnet publish "APIGateway/APIGateway.csproj" -c Release -o /app /p:UseAppHost=false

# Run stage
FROM base AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "APIGateway.dll"]