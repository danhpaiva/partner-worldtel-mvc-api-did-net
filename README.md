# 📘 Partner.WorldTel.Did – API de Gerenciamento de DIDs Internacionais

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-6DB33F?style=for-the-badge&logo=ef&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)



Este repositório contém a **Partner.WorldTel.Did.Api**, uma aplicação ASP.NET Core voltada para o gerenciamento de **DIDs internacionais**, incluindo autenticação de parceiros, geração de números, consulta de DIDs e persistência via Entity Framework Core em banco SQLite.

A API foi desenvolvida utilizando **boas práticas de arquitetura**, separação em camadas lógicas, DTOs bem definidos e migrações controladas por EF Core.

---

## 🚀 Tecnologias Utilizadas

* **.NET 10 / ASP.NET Core**
* **Entity Framework Core** + Migrations
* **SQLite** como banco principal
* **JWT Authentication**
* **Injeção de dependência (DI)**
* **RESTful Controllers**
* **Docker** + Dockerfile para deploy containerizado

---

## 📂 Estrutura do Projeto

```
Partner.WorldTel.Did/
│
├── Partner.WorldTel.Did.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── InternationalDidsController.cs
│   │
│   ├── Data/
│   │   └── AppDbContext.cs
│   │
│   ├── DTO/
│   │   ├── CreateDidFromNumberRequest.cs
│   │   ├── LoginRequest.cs
│   │   └── LoginResponse.cs
│   │
│   ├── Enum/
│   │   └── DidStatus.cs
│   │
│   ├── Interface/
│   │   ├── IAuthService.cs
│   │   └── IDidGeneratorService.cs
│   │
│   ├── Migrations/
│   ├── appsettings.json
│   ├── Program.cs
│   ├── Dockerfile
│   └── WorldTelDatabase.db
│
└── Partner.WorldTel.Did.slnx
```

---

## 🔑 Autenticação

A API utiliza **JWT Bearer Token**.

### **Rota de Login**

```
POST /auth/login
```

#### Payload:

```json
{
  "username": "admin",
  "password": "admin123"
}
```

#### Resposta:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6ImY1NzM0YzA3LTBlZGQtNDk1Yi04NGZhLTYwY2IyYjllOTEyMCIsInVuaXF1ZV9uYW1lIjoiYWRtaW4iLCJyb2xlIjoiQWRtaW4iLCJ1c2VyaWQiOiIxIiwibmJmIjoxNzYzMjMwMDA4LCJleHAiOjE3NjMyMzA5MDgsImlhdCI6MTc2MzIzMDAwOCwiaXNzIjoiUGFydG5lci5Xb3JsZFRlbC5EaWQuQXBpIiwiYXVkIjoiaHR0cHM6Ly9hcGkud29ybGR0ZWwucGFydG5lciJ9.TDdfu_sSN3JsjA6Bnao0zmv4PQZq_pdCh8eCM-mckx8",
  "refreshToken": "eUmZqDXz2bNq+mAkWZh1jsoSFLzUGaalviU3i7V6p2OWe9VL5eJwiY8KsABs0tv7qmyHg6QvKrf0lPs9W3Zduw==",
  "expiresAt": "2025-11-15T18:21:48.5589803Z",
  "role": "Admin",
  "username": "admin"
}
```

Utilize o token no header:

```
Authorization: Bearer <token>
```

---

## 🌍 Endpoints Principais

### **Gerar um DID a partir de um número**

```
POST /internationaldids/create-from-number
```

Payload exemplo:

```json
{
  "e164Number": "+4930123456789",
  "createdBy": "danielpaiva"
}
```

---

### **Listar todos os DIDs gerados**

```
GET /internationaldids
```

---

### **Filtrar DIDs por status**

```
GET /internationaldids/status/{status}
```

Status disponíveis (enum `DidStatus`):

~~~
public enum DidStatus
{
    Pending,
    Active,
    Suspended,
    Cancelled,
    PortedOut
}
~~~

---

## 🗄️ Banco de Dados

A API utiliza:

* `SQLite`
* `EF Core` com migrations versionadas
* Arquivo físico incluído: `WorldTelDatabase.db`

Execute a migração (se necessário):

```bash
dotnet ef database update
```

---

## 🐳 Docker

O Dockerfile já está configurado para build e execução da API.

### **Build**

```bash
docker build -t worldtel-did-api .
```

### **Run**

```bash
docker run -p 8080:80 worldtel-did-api
```

---

## 🧪 Testando com o arquivo `.http`

O arquivo:

```
Partner.WorldTel.Did.Api/Partner.WorldTel.Did.Api.http
```

contém requisições prontas para teste via:

* Visual Studio Code (REST Client)
* Rider (HTTP Client)
* Visual Studio (HTTP Editor)

---

## 📜 Como Executar Localmente

1. Clone o repositório:

```bash
git clone https://github.com/seu-user/Partner.WorldTel.Did.git
```

2. Acesse a pasta do projeto:

```bash
cd Partner.WorldTel.Did/Partner.WorldTel.Did.Api
```

3. Execute:

```bash
dotnet run
```

A API ficará disponível em:

```
https://localhost:7264
http://localhost:5049
```

---

## 🧱 Roadmap (Sugestões Futuras)

* [ ] Implementar refresh token
* [ ] Registrar logs com Serilog
* [ ] Criar testes unitários com xUnit
* [ ] Criar testes integrados utilizando SQLite InMemory
* [ ] Documentação Swagger/OpenAPI completa

---

## 👨‍💻 Autor

**Daniel Paiva**
Desenvolvedor .NET | Professor Universitário

![Stars](https://img.shields.io/github/stars/danhpaiva/Partner.WorldTel.Did?style=for-the-badge)
![Forks](https://img.shields.io/github/forks/danhpaiva/Partner.WorldTel.Did?style=for-the-badge)
![Issues](https://img.shields.io/github/issues/danhpaiva/Partner.WorldTel.Did?style=for-the-badge)
