# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN mkdir -p /app/wwwroot/uploads \
    && chown -R $APP_UID /app/wwwroot/uploads \
    && chmod -R 775 /app/wwwroot/uploads
USER $APP_UID
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InternshipPlatform.Api/InternshipPlatform.Api.csproj", "InternshipPlatform.Api/"]
COPY ["InternshipPlatform.Infrastructure/InternshipPlatform.Infrastructure.csproj", "InternshipPlatform.Infrastructure/"]
COPY ["InternshipPlatform.Application/InternshipPlatform.Application.csproj", "InternshipPlatform.Application/"]
COPY ["InternshipPlatform.Domain/InternshipPlatform.Domain.csproj", "InternshipPlatform.Domain/"]
RUN dotnet restore "./InternshipPlatform.Api/InternshipPlatform.Api.csproj"
COPY . .
WORKDIR "/src/InternshipPlatform.Api"
RUN dotnet build "./InternshipPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InternshipPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternshipPlatform.Api.dll"]