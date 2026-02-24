#!/bin/bash
set -e

echo "Ожидание готовности SQL Server..."
max_retries=30
counter=0

# Ждем пока SQL Server будет готов
until /opt/mssql-tools18/bin/sqlcmd -S sql-server -U sa -P YourStrong@Password -C -Q "SELECT 1" &> /dev/null
do
    counter=$((counter+1))
    if [ $counter -gt $max_retries ]; then
        echo "Превышено время ожидания SQL Server"
        exit 1
    fi
    echo "Ожидание SQL Server... (попытка $counter/$max_retries)"
    sleep 2
done

echo "SQL Server готов! Запускаем приложение..."

# Запускаем приложение
dotnet MvcMovie.dll