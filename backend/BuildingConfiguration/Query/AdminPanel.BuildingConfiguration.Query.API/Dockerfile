FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 8080
COPY Shared/AdminPanel.Shared/*.csproj Shared/AdminPanel.Shared/
COPY BuildingConfiguration/Common/*.csproj BuildingConfiguration/Common/
COPY BuildingConfiguration/CQRS.Core/*.csproj BuildingConfiguration/CQRS.Core/
COPY BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.API/*.csproj BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.API/
COPY BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Application/*.csproj BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Application/
COPY BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Domain/*.csproj BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Domain/
COPY BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Persistence/*.csproj BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.Persistence/
RUN dotnet restore BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.API/*.csproj
COPY . .
RUN dotnet publish BuildingConfiguration/Query/AdminPanel.BuildingConfiguration.Query.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AdminPanel.BuildingConfiguration.Query.API.dll" ]