#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FinChatter.BotService/FinChatter.BotService.csproj", "src/FinChatter.BotService/"]
COPY ["src/FinChatter.Application/FinChatter.Application.csproj", "src/FinChatter.Application/"]
COPY ["src/FinChatter.API.Contracts/FinChatter.API.Contracts.csproj", "src/FinChatter.API.Contracts/"]
COPY ["src/FinChatter.Infrastructure/FinChatter.Infrastructure.csproj", "src/FinChatter.Infrastructure/"]
COPY ["src/StockService/StockService.csproj", "src/StockService/"]
RUN dotnet restore "src/FinChatter.BotService/FinChatter.BotService.csproj"
COPY . .
WORKDIR "/src/src/FinChatter.BotService"
RUN dotnet build "FinChatter.BotService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinChatter.BotService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinChatter.BotService.dll"]