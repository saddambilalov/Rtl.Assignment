#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Console/Rtl.Assignment.Scraper/Rtl.Assignment.Scraper.csproj", "src/Console/Rtl.Assignment.Scraper/"]
COPY ["src/Rtl.Assignment.Infrastructure/Rtl.Assignment.Infrastructure.csproj", "src/Rtl.Assignment.Infrastructure/"]
COPY ["src/Rtl.Assignment.Domain/Rtl.Assignment.Domain.csproj", "src/Rtl.Assignment.Domain/"]
RUN dotnet restore "src/Console/Rtl.Assignment.Scraper/Rtl.Assignment.Scraper.csproj"
COPY . .
WORKDIR "/src/src/Console/Rtl.Assignment.Scraper"
RUN dotnet build "Rtl.Assignment.Scraper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rtl.Assignment.Scraper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rtl.Assignment.Scraper.dll"]