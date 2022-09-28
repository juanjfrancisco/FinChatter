#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FinChatter.WebUI/FinChatter.WebUI.csproj", "src/FinChatter.WebUI/"]
COPY ["src/FinChatter.WebUI.Core/FinChatter.WebUI.Core.csproj", "src/FinChatter.WebUI.Core/"]
COPY ["src/FinChatter.API.Contracts/FinChatter.API.Contracts.csproj", "src/FinChatter.API.Contracts/"]
COPY ["src/FinChatter.API.Client/FinChatter.API.Client.csproj", "src/FinChatter.API.Client/"]


RUN dotnet restore "src/FinChatter.WebUI/FinChatter.WebUI.csproj"
COPY . .
WORKDIR "/src/src/FinChatter.WebUI"
RUN dotnet build "FinChatter.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinChatter.WebUI.csproj" -c Release -o /app/publish


FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY src/FinChatter.WebUI/nginx.conf /etc/nginx/nginx.conf