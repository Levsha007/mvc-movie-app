FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["MvcMovie.csproj", "."]
RUN dotnet restore "MvcMovie.csproj"

COPY . .
RUN dotnet publish "MvcMovie.csproj" -c Release -o /app/publish

# Финальный образ с установкой mssql-tools
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Устанавливаем mssql-tools для healthcheck
RUN apt-get update && \
    apt-get install -y curl gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools18 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Добавляем sqlcmd в PATH
ENV PATH="$PATH:/opt/mssql-tools18/bin"

COPY --from=build /app/publish .

EXPOSE 80

# Простой скрипт ожидания
COPY wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

CMD ["./wait-for-sql.sh"]