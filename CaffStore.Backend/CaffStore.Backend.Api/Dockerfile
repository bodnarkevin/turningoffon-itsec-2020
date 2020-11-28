#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y libgdiplus build-essential
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CaffStore.Backend.Api/CaffStore.Backend.Api.csproj", "CaffStore.Backend.Api/"]
RUN dotnet restore "CaffStore.Backend.Api/CaffStore.Backend.Api.csproj"
COPY . .
WORKDIR "/src/CaffStore.Backend.Api"
RUN dotnet build "CaffStore.Backend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CaffStore.Backend.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaffStore.Backend.Api.dll"]