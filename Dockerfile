# Stage 1: Build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore ./FlexyBox.web/FlexyBox.web.csproj

# Build the Blazor WebAssembly project in Release mode
RUN dotnet publish ./FlexyBox.web/FlexyBox.web.csproj -c Release -o /out

# Stage 2: Create an Nginx image with the Blazor WebAssembly app
FROM nginx:alpine AS final

WORKDIR /usr/share/nginx/html

# Remove default Nginx static resources
RUN rm -rf ./*

# Copy the Blazor WebAssembly app from the build container
COPY --from=build /out/wwwroot .

# Copy custom Nginx config file
COPY FlexyBox.web/nginx.conf /etc/nginx/nginx.conf

# Copy SSL certificate and private key
COPY cert.pem /etc/ssl/certs/cert.pem
COPY key.pem /etc/ssl/private/key.pem

# Expose ports 80 and 443
EXPOSE 80
EXPOSE 443

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
