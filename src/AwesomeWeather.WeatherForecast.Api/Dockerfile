#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
#ENV ASPNETCORE_URLS="http://+:80;https://+:443"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["AwesomeWeather.WeatherForecast.Api.csproj", "AwesomeWeather.WeatherForecast.Api/"]
RUN dotnet restore "AwesomeWeather.WeatherForecast.Api/AwesomeWeather.WeatherForecast.Api.csproj"
COPY . ./AwesomeWeather.WeatherForecast.Api
WORKDIR "/src/AwesomeWeather.WeatherForecast.Api"
RUN dotnet build "AwesomeWeather.WeatherForecast.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AwesomeWeather.WeatherForecast.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AwesomeWeather.WeatherForecast.Api.dll"]