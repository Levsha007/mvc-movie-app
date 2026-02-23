# Запуск MVC Movie App в Docker (без HTTPS)

## Предварительные требования
- Установленный Docker Desktop

## Запуск с помощью Docker Compose

```bash
# Сборка и запуск всех контейнеров
docker-compose up --build

# Запуск в фоновом режиме
docker-compose up -d --build

# Остановка контейнеров
docker-compose down

# Остановка с удалением томов (удалит БД)
docker-compose down -v