# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

RUN apt-get update && apt-get install -y ca-certificates curl apt-transport-https

# Copy solution and project files
COPY profile-api.sln .
COPY ProfileApi.csproj .

# Restore dependencies
#RUN dotnet restore profile-api.sln
RUN dotnet restore ProfileApi.csproj

# Copy everything else
COPY . .

# Publish the app
RUN ls -R /src
RUN dotnet publish ProfileApi.csproj -c Release -o /app/publish
RUN ls -R /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ProfileApi.dll"]
