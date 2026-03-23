FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["MvcMovie.csproj", "."]
RUN dotnet restore "MvcMovie.csproj"

COPY . .
RUN dotnet publish "MvcMovie.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["dotnet", "MvcMovie.dll"]