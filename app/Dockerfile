FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.sln .
COPY *.props .
COPY src/Shared/*.csproj ./src/Shared/
COPY src/WebApi/*.csproj ./src/WebApi/
COPY src/WebClient/*.csproj ./src/WebClient/
RUN dotnet restore
COPY . .
RUN dotnet build src/WebApi -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/WebApi -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lsquared.Apps.WeatherForecast.WebApi.dll"]
