FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Удаляем старые миграции и восстанавливаем зависимости
COPY ["MvcMovie.csproj", "."]
RUN dotnet restore "MvcMovie.csproj"

# Копируем все файлы и удаляем папку Migrations
COPY . .
RUN rm -rf Migrations

# Публикуем приложение
RUN dotnet publish "MvcMovie.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Устанавливаем SQLite и другие зависимости
RUN apt-get update && \
    apt-get install -y sqlite3 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["dotnet", "MvcMovie.dll"]