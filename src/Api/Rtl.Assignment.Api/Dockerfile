#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Api/Rtl.Assignment.Api/Rtl.Assignment.Api.csproj", "src/Api/Rtl.Assignment.Api/"]
COPY ["src/Rtl.Assignment.Infrastructure/Rtl.Assignment.Infrastructure.csproj", "src/Rtl.Assignment.Infrastructure/"]
COPY ["src/Rtl.Assignment.Domain/Rtl.Assignment.Domain.csproj", "src/Rtl.Assignment.Domain/"]
COPY ["src/Api/Rtl.Assignment.Api.Abstractions/Rtl.Assignment.Api.Abstractions.csproj", "src/Api/Rtl.Assignment.Api.Abstractions/"]
RUN dotnet restore "src/Api/Rtl.Assignment.Api/Rtl.Assignment.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/Rtl.Assignment.Api"
RUN dotnet build "Rtl.Assignment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rtl.Assignment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rtl.Assignment.Api.dll"]