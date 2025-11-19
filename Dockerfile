FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY *.csproj .

COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

EXPOSE 8000

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet","lista-de-tarefa-api.dll" ]