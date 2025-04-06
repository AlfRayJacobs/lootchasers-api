# syntax=docker/dockerfile:1

# Define the .NET version to use
ARG DOTNET_VERSION=8.0

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy and restore dependencies
COPY LootchasersAPI.csproj ./
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.cache/msbuild \
    dotnet restore

# Copy the rest of the application
COPY . .

# Build the application
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.cache/msbuild \
    dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.cache/msbuild \
    dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set a non-root user for security
RUN useradd -m appuser
USER appuser

# Define the entry point
ENTRYPOINT ["dotnet", "LootchasersAPI.dll"]