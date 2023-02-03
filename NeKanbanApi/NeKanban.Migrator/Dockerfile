FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NeKanban.Migrator/NeKanban.Migrator.csproj", "NeKanban.Migrator/"]
COPY ["NeKanban.Data/NeKanban.Data.csproj", "NeKanban.Data/"]
COPY ["NeKanban.Common/NeKanban.Common.csproj", "NeKanban.Common/"]
COPY ["NeKanban.Security/NeKanban.Security.csproj", "NeKanban.Security/"]
RUN dotnet restore "NeKanban.Migrator/NeKanban.Migrator.csproj"
COPY . .
WORKDIR "/src/NeKanban.Migrator"
RUN dotnet build "NeKanban.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NeKanban.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeKanban.Migrator.dll"]
