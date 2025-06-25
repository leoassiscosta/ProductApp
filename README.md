# ProductApp Aspire Demo

Este projeto demonstra uma aplicação distribuída utilizando .NET Aspire, composta por:

- **API** (ASP.NET Core + FastEndpoints)
- **Blazor Web** (interface)
- **PostgreSQL** (container)
- Orquestração com **Aspire AppHost**

---

## Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

---

## Build da imagem da API

Antes de rodar a aplicação com Aspire, você deve construir a imagem da API:

cd ProductApp.ApiService
docker build -t my-api-image .

> Certifique-se de que o `Dockerfile` está nesta pasta.

---

## Como rodar

Execute o AppHost com:

dotnet run --project ProductApp.AppHost

Isso irá:

- Subir o container do PostgreSQL
- Rodar a API via container
- Iniciar o frontend Blazor
- Exibir o Aspire Dashboard

---

## Endpoints úteis

- API Swagger: http://localhost:8000/swagger
- Blazor Web: http://localhost:port (definido no dashboard)
- Aspire Dashboard: http://localhost:17125 (ou como indicado no console)

---

## Testes

A aplicação suporta testes com TestContainers.
Execute:

dotnet test ProductApp.ApiService.Tests

