FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

ARG CONFIGURATION=Release

# Restore dependencies
COPY ["ftrip.io.email-service.contracts/*.csproj", "ftrip.io.email-service.contracts/package*.json", "./ftrip.io.email-service.contracts/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.email-service.contracts/ftrip.io.email-service.contracts.csproj

COPY ["ftrip.io.email-service/*.csproj", "ftrip.io.email-service/package*.json", "./ftrip.io.email-service/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.email-service/ftrip.io.email-service.csproj

# Publish
COPY . .

RUN dotnet publish -c $CONFIGURATION -o out ftrip.io.email-service/ftrip.io.email-service.csproj
 
# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
WORKDIR /app

RUN addgroup --system ftrip-io
RUN adduser --system email-service

COPY --from=build --chown=email-service:ftrip-io --chmod=777 /build/out .

EXPOSE 80

USER email-service

ENTRYPOINT ["dotnet", "ftrip.io.email-service.dll"]