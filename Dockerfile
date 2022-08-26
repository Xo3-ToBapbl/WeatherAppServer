FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WeatherAppServer/WeatherAppServer.csproj", "WeatherAppServer/"]
RUN dotnet restore "WeatherAppServer/WeatherAppServer.csproj"
COPY . .
WORKDIR "/src/WeatherAppServer"
ARG BUILD_CONFIGURATION
RUN dotnet build "WeatherAppServer.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "WeatherAppServer.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherAppServer.dll"]