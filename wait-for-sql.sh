#!/bin/bash
# Ожидание готовности SQL Server
echo "Ожидание готовности SQL Server..."
for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P YourStrong@Password -Q "SELECT 1" &> /dev/null
    if [ $? -eq 0 ]
    then
        echo "SQL Server готов!"
        break
    else
        echo "Ожидание SQL Server... (попытка $i)"
        sleep 2
    fi
done

# Запуск приложения
dotnet MvcMovie.dll