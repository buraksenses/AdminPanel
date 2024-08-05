FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 8080
COPY Shared/AdminPanel.Shared/*.csproj Shared/AdminPanel.Shared/
COPY BuildingConfiguration/Common/*.csproj BuildingConfiguration/Common/
COPY BuildingConfiguration/CQRS.Core/*.csproj BuildingConfiguration/CQRS.Core/
COPY BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/*.csproj BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/
COPY BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Application/*.csproj BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Application/
COPY BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Domain/*.csproj BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Domain/
COPY BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Persistence/*.csproj BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.Persistence/
RUN dotnet restore BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/*.csproj
COPY . .
RUN dotnet publish BuildingConfiguration/Command/AdminPanel.BuildingConfiguration.Command.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AdminPanel.BuildingConfiguration.Command.API.dll" ]