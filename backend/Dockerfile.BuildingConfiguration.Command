#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/AdminPanel.BuildingConfiguration.Command.API.csproj", "BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/"]
COPY ["Shared/AdminPanel.Shared/AdminPanel.Shared.csproj", "Shared/AdminPanel.Shared/"]
COPY ["BuildingConfiguration/Common/Common.csproj", "BuildingConfiguration/Common/"]
COPY ["BuildingConfiguration/CQRS.Core/CQRS.Core.csproj", "BuildingConfiguration/CQRS.Core/"]
COPY ["BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Application/AdminPanel.BuildingConfiguration.Command.Application.csproj", "BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Application/"]
COPY ["BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Domain/AdminPanel.BuildingConfiguration.Command.Domain.csproj", "BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Domain/"]
COPY ["BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Persistence/AdminPanel.BuildingConfiguration.Command.Persistence.csproj", "BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Persistence/"]
RUN dotnet restore "./BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/AdminPanel.BuildingConfiguration.Command.API.csproj"
COPY . .
WORKDIR "/src/BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API"
RUN dotnet build "./AdminPanel.BuildingConfiguration.Command.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdminPanel.BuildingConfiguration.Command.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminPanel.BuildingConfiguration.Command.API.dll"]