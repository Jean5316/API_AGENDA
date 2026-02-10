# 📒 API de Agenda – ASP.NET Core

API REST desenvolvida em **ASP.NET Core (.NET 8)** para gerenciamento de contatos de uma agenda, utilizando **Entity Framework Core** com **SQLite** e arquitetura organizada com **Repository Pattern**.

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- Swagger (OpenAPI)
- C#
- Git / GitHub

---

## 📁 Estrutura do Projeto

API_AGENDA
├── Controllers
├── Context
├── Models
├── DTOs
├── Repository
├── Migrations
├── DB
└── Program.cs


---

## 📌 Funcionalidades

- Criar contato
- Listar contatos
- Buscar contato por ID
- Atualizar contato
- Remover contato (Hard delete)
- Marcar contato como favorito
- Organização por categoria = NAO IMPLEMENTADO

---

## 📡 Endpoints

| Método | Rota | Descrição |
|------|------|----------|
| GET | `/api/contatos` | Lista todos os contatos |
| GET | `/api/contatos/Favoritos` | Lista todos os contatos Favoritos|
| GET | `/api/contatos/{id}` | Busca contato por ID |
| POST | `/api/contatos` | Cria um novo contato |
| PUT | `/api/contatos/{id}` | Atualiza um contato |
| DELETE | `/api/contatos/{id}` | Remove um contato |



---

## ▶️ Como executar o projeto

### Pré-requisitos
- .NET SDK 8+

### Passos

```bash
git clone https://github.com/SEU_USUARIO/api-agenda-aspnet.git
cd API_AGENDA
dotnet restore
dotnet ef database update
dotnet run


