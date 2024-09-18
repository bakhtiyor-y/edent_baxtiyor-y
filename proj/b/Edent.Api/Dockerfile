FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Edent.Api/Edent.Api.csproj", "Edent.Api/"]
RUN dotnet restore "Edent.Api/Edent.Api.csproj"
COPY . ./Edent.Api
WORKDIR "/src/Edent.Api"
RUN dotnet build "Edent.Api/Edent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Edent.Api/Edent.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Edent.Api.dll"]


