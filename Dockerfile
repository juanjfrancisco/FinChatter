# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./src/FinChatter.API/FinChatter.API.csproj" --disable-parallel
RUN dotnet publish "./src/FinChatter.API/FinChatter.API.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5080

ENTRYPOINT ["dotnet", "FinChatter.API.dll"]