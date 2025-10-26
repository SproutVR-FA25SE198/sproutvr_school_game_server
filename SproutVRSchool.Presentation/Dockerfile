# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 7701

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["SproutVRSchool.Infrastructure/SproutVRSchool.Infrastructure.csproj", "SproutVRSchool.Infrastructure/"]
COPY ["SproutVRSchool.Presentation/SproutVRSchool.Presentation.csproj", "SproutVRSchool.Presentation/"]
COPY ["SproutVRSchool.Application/SproutVRSchool.Application.csproj", "SproutVRSchool.Application/"]
COPY ["SproutVRSchool.Domain/SproutVRSchool.Domain.csproj", "SproutVRSchool.Domain/"]

RUN dotnet restore "./SproutVRSchool.Presentation/SproutVRSchool.Presentation.csproj"
COPY . .
WORKDIR "/src/SproutVRSchool.Presentation"
RUN dotnet build "./SproutVRSchool.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SproutVRSchool.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir /app/storage
RUN chmod -R 777 /app/storage

ENTRYPOINT ["dotnet", "SproutVRSchool.Presentation.dll"]