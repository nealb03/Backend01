# ------------------------------------------------------------
# STAGE 1: Build and publish the application (using .NET 8 SDK)
# ------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies (layer caching)
COPY ["MyWebApi.csproj", "./"]
RUN dotnet restore "MyWebApi.csproj"

# Copy the remaining source code and publish the app
COPY . .
RUN dotnet publish "MyWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------------------------------------------------------
# STAGE 2: Runtime image (using .NET 8 ASP.NET runtime)
# ------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish ./

# ------------------------------------------------------------
# Configuration for ECS / ALB compatibility
# ------------------------------------------------------------

# Make Kestrel listen on all interfaces at port 80
ENV ASPNETCORE_URLS=http://0.0.0.0:80

# Optionally disable ASP.NET telemetry messages
ENV DOTNET_PRINT_TELEMETRY_MESSAGE=false

# Expose the port ECS / ALB will forward to
EXPOSE 80

# Optionally enable container-level health check (if ALB doesn’t handle)
# HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
#   CMD curl --fail http://localhost:80/health || exit 1

# ------------------------------------------------------------
# Start the application
# ------------------------------------------------------------
ENTRYPOINT ["dotnet", "MyWebApi.dll"]