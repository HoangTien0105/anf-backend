# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ANF.Application/ANF.Application.csproj", "ANF.Application/"]
COPY ["ANF.Core/ANF.Core.csproj", "ANF.Core/"]
COPY ["ANF.Infrastructure/ANF.Infrastructure.csproj", "ANF.Infrastructure/"]
COPY ["ANF.Service/ANF.Service.csproj", "ANF.Service/"]
RUN dotnet restore "./ANF.Application/ANF.Application.csproj"
COPY . .
WORKDIR "/src/ANF.Application"
RUN dotnet build "./ANF.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ANF.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ANF.Application.dll"]