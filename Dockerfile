FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app


COPY ["../cert.pfx", "/https/cert.pfx"]

ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=MyPass123

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["FlexyBox.web/FlexyBox.web.csproj", "FlexyBox.web/"]
RUN dotnet restore "./FlexyBox.web/FlexyBox.web.csproj"

COPY . .

WORKDIR "/src/FlexyBox.web"
RUN dotnet build "./FlexyBox.web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./FlexyBox.web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine

WORKDIR /usr/share/nginx/html
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish/wwwroot .
COPY FlexyBox.web/nginx.conf /etc/nginx/nginx.conf
#WORKDIR /app

#COPY --from=publish /app/publish .

#ENTRYPOINT ["dotnet", "FlexyBox.web.dll"]
