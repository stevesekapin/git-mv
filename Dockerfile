# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy entire repo
COPY . .

# Restore using solution file
RUN dotnet restore git-mv.sln

# Publish console app
RUN dotnet publish DatabaseConsoleApp/DatabaseConsoleApp.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "DatabaseConsoleApp.dll"]
