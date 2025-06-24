# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RESTfulWebAPITask1/RESTfulWebAPITask1.csproj", "RESTfulWebAPITask1/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "./RESTfulWebAPITask1/RESTfulWebAPITask1.csproj"
COPY . .
WORKDIR "/src/RESTfulWebAPITask1"
RUN dotnet build "./RESTfulWebAPITask1.csproj" -c %BUILD_CONFIGURATION% -o /app/build


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RESTfulWebAPITask1.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RESTfulWebAPITask1.dll"]