#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContatoAPI/ContatoAPI.csproj", "ContatoAPI/"]
COPY ["Aplicacao/Aplicacao.csproj", "Aplicacao/"]
COPY ["Infraestrutura/Infraestrutura.csproj", "Infraestrutura/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "./ContatoAPI/ContatoAPI.csproj"
COPY . .
WORKDIR "/src/ContatoAPI"
RUN dotnet build "./ContatoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ContatoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContatoAPI.dll"]