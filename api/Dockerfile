# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80      
EXPOSE 443     

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY WebApplication1/WebApplication1.csproj WebApplication1/
COPY Core/Core.csproj Core/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY BusinessLogic/BusinessLogic.csproj BusinessLogic/

# Restore dependencies
RUN dotnet restore "WebApplication1/WebApplication1.csproj"

# Copy all source files
COPY . .

# Set the working directory for the build
WORKDIR "/src/WebApplication1"

# Build the project
RUN dotnet build "WebApplication1.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Set environment variable for ASP.NET Core to use port 80
ENV ASPNETCORE_URLS=http://+:80  

# Ensure the SQLite database file is copied and persists inside the container
COPY WebApplication1/taskdb.sqlite /app/taskdb.sqlite

# Set the entry point
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
