# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 5002


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PostService/PostService.API.csproj", "PostService/"]
COPY ["PostService.Application/PostService.Application.csproj", "PostService.Application/"]
COPY ["PostService.Core/PostService.Core.csproj", "PostService.Core/"]
COPY ["PostService.DataAccess/PostService.DataAccess.csproj", "PostService.DataAccess/"]
COPY ["PostService.GRPC.API/PostService.GRPC.Server.csproj", "PostService.GRPC.API/"]
COPY ["PostService.RabbitMQ/PostService.RabbitMQ.csproj", "PostService.RabbitMQ/"]
RUN dotnet restore "./PostService/PostService.API.csproj"
COPY . .
WORKDIR "/src/PostService"
RUN dotnet build "./PostService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PostService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Создаем директорию для миграций, если её нет, и копируем напрямую из исходников
COPY --from=build /src/PostService.DataAccess/Migrations ./Migrations
ENTRYPOINT ["dotnet", "PostService.API.dll"]