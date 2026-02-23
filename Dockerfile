FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["MvcMovie.csproj", "."]
RUN dotnet restore "MvcMovie.csproj"

# Копируем все остальные файлы
COPY . .

# Публикуем приложение
RUN dotnet publish "MvcMovie.csproj" -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Устанавливаем необходимые инструменты для ожидания SQL Server
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 80

# Скрипт для ожидания готовности SQL Server
COPY wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

ENTRYPOINT ["./wait-for-sql.sh"]