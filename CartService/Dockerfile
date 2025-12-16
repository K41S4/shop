FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# restore
COPY ["CartService/CartApp.csproj", "CartService/"]
RUN dotnet restore "CartService/CartApp.csproj"

# build and publish
COPY ["CartService", "CartService/"]
RUN dotnet publish "CartService/CartApp.csproj" -c Release -o /app /p:UseAppHost=false

# Run stage
FROM base AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CartApp.dll"]