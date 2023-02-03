FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NeKanban.Seeder/NeKanban.Seeder.csproj", "NeKanban.Seeder/"]
COPY ["NeKanban.Common/NeKanban.Common.csproj", "NeKanban.Common/"]
COPY ["NeKanban.Security/NeKanban.Security.csproj", "NeKanban.Security/"]
COPY ["NeKanban.Data/NeKanban.Data.csproj", "NeKanban.Data/"]
COPY ["NeKanban.Logic/NeKanban.Logic.csproj", "NeKanban.Logic/"]
COPY ["NeKanban.FrameworkExceptions/NeKanban.FrameworkExceptions.csproj", "NeKanban.FrameworkExceptions/"]
RUN dotnet restore "NeKanban.Seeder/NeKanban.Seeder.csproj"
COPY . .
WORKDIR "/src/NeKanban.Seeder"
RUN dotnet build "NeKanban.Seeder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NeKanban.Seeder.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeKanban.Seeder.dll"]
