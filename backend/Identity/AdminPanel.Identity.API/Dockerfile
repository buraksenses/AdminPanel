FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 8080
COPY Shared/AdminPanel.Shared/*.csproj Shared/AdminPanel.Shared/
COPY Identity/AdminPanel.Identity.API/*.csproj Identity/AdminPanel.Identity.API/
COPY Identity/AdminPanel.Identity.Application/*.csproj Identity/AdminPanel.Identity.Application/
COPY Identity/AdminPanel.Identity.Domain/*.csproj Identity/AdminPanel.Identity.Domain/
COPY Identity/AdminPanel.Identity.Persistence/AdminPanel.Identity.Persistence.csproj Identity/AdminPanel.Identity.Persistence/
RUN dotnet restore Identity/AdminPanel.Identity.API/*.csproj
COPY . .
RUN dotnet publish Identity/AdminPanel.Identity.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AdminPanel.Identity.API.dll" ]