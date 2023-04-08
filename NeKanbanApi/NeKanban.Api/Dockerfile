FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NeKanban.Api/NeKanban.Api.csproj", "NeKanban.Api/"]
COPY ["NeKanban.Logic/NeKanban.Logic.csproj", "NeKanban.Logic/"]
COPY ["NeKanban.Common/NeKanban.Common.csproj", "NeKanban.Common/"]
COPY ["Batteries/Batteries.csproj", "Batteries/"]
COPY ["NeKanban.Security/NeKanban.Security.csproj", "NeKanban.Security/"]
COPY ["NeKanban.Data/NeKanban.Data.csproj", "NeKanban.Data/"]
RUN dotnet restore "NeKanban.Api/NeKanban.Api.csproj"
COPY . .
WORKDIR "/src/NeKanban.Api"
RUN dotnet build "NeKanban.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "NeKanban.Api.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeKanban.Api.dll"]
