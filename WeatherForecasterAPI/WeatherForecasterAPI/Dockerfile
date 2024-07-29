#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#Run VS
#COPY ["WeatherForecasterAPI/WeatherForecasterAPI.csproj", "WeatherForecasterAPI/"]
#Run Compose
COPY ["WeatherForecasterAPI/WeatherForecasterAPI/WeatherForecasterAPI.csproj", "WeatherForecasterAPI/"]
RUN dotnet restore "./WeatherForecasterAPI/WeatherForecasterAPI.csproj"
COPY . .
WORKDIR "/src/WeatherForecasterAPI"
#Run VS
#RUN dotnet build "./WeatherForecasterAPI.csproj" -c $BUILD _CONFIGURATION -o /app/build
#Run Compose
RUN dotnet build "./WeatherForecasterAPI/WeatherForecasterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
#Run VS
#RUN dotnet publish "./WeatherForecasterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#Run Compose
RUN dotnet publish "./WeatherForecasterAPI/WeatherForecasterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecasterAPI.dll"]