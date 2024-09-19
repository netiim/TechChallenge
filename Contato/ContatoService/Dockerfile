FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContatoService/ContatoAPI/ContatoService.API.csproj", "ContatoService/ContatoAPI/"]
COPY ["ContatoService/Aplicacao/ContatoService.Aplicacao.csproj", "ContatoService/Aplicacao/"]
COPY ["ContatoService/Infraestrutura/ContatoService.Infraestrutura.csproj", "ContatoService/Infraestrutura/"]
COPY ["ContatoService/Core/ContatoService.Core.csproj", "ContatoService/Core/"]
RUN dotnet restore "./ContatoService/ContatoAPI/ContatoService.API.csproj"
COPY . .
WORKDIR "/src/ContatoService/ContatoAPI"
RUN dotnet build "./ContatoService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ContatoService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContatoService.API.dll"]