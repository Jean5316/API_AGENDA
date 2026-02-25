# 📌 API Agenda — ASP.NET Core + JWT

[![ .NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=flat-square&logo=entity-framework&logoColor=white)](https://docs.microsoft.com/pt-br/ef/core/)
[![JWT](https://img.shields.io/badge/JWT-black?style=flat-square&logo=JSON%20Web%20Tokens&logoColor=white)](https://jwt.io/)
[![SQLite](https://img.shields.io/badge/SQLite-003B57?style=flat-square&logo=sqlite&logoColor=white)](https://www.sqlite.org/index.html)
[![Swagger/Scalar](https://img.shields.io/badge/Scalar-8B5CF6?style=flat-square)](https://scalar.com/)
[![Testes](https://img.shields.io/badge/Testes-MSTest-004880?style=flat-square&logo=microsoft&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/core/testing/)

API REST desenvolvida em **ASP.NET Core** com autenticação via **JWT (JSON Web Token)**.
Esta API é consumida por um frontend em **Angular 21**, responsável pelo login, proteção de rotas e gerenciamento de contatos.

---

## 🚀 Tecnologias utilizadas

- ASP.NET Core 10.0
- C#
- Entity Framework Core
- JWT (JSON Web Token) com Refresh Token
- Scalar (OpenAPI/Swagger alternativo)
- SQLite
- MSTest (testes automatizados)
- Arquitetura Repository Pattern + Service Layer

---

## 🎯 Features Implementadas

### ✅ Autenticação e Autorização
- [x] Login com email e senha
- [x] Geração de JWT Token
- [x] Refresh Token para renovação de sessão
- [x] Roles & Permissões (Admin, User)
- [x] Hash de senhas com PBKDF2

### ✅ Gerenciamento de Contatos
- [x] CRUD completo de contatos
- [x] Relacionamento entre Contatos e Usuário
- [x] Busca por nome
- [x] Paginação de resultados
- [x] Contatos favoritos
- [x] Validações de dados

### ✅ Área Administrativa
- [x] Listar usuários
- [x] Alterar usuários
- [x] Deletar usuários (requer role Admin)

### ✅ Estrutura e Qualidade
- [x] Repository Pattern
- [x] Service Layer
- [x] DTOs para transferência de dados
- [x] Injeção de dependência
- [x] Configuração via appsettings.json
- [x] Middleware CORS para Angular

---

## 📁 Estrutura do Projeto

```
API_AGENDA/
├── API/                          # Projeto principal
│   ├── Controllers/              # Controladores da API
│   │   ├── AuthController.cs      # Endpoints de autenticação
│   │   ├── ContatosController.cs # Endpoints de contatos
│   │   └── AdminController.cs    # Endpoints administrativos
│   ├── Services/                 # Camada de serviços
│   ├── Repository/               # Repositórios (acesso a dados)
│   ├── Models/                   # Entidades do banco
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Context/                  # DbContext do EF Core
│   ├── Migrations/               # Migrações do banco
│   └── Program.cs                # Configuração da aplicação
├── Teste/                        # Projeto de testes
└── README.md                     # Este arquivo
```

---

## ⚙️ Configuração

### appsettings.json

```
json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "ConexaoSqlite": "Data Source=DB/agenda.db"
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_AQUI",
    "Issuer": "API_AGENDA",
    "Audience": "API_AGENDA_USUARIOS"
  }
}
```

> ⚠️ **Importante**: Altere a chave JWT (`Key`) para um valor seguro em produção!

---

## 📋 Requisitos

- .NET 10.0 SDK
- Visual Studio 2022+ ou VS Code (extensões C#)
- (Opcional) Docker

---

## ▶️ Como executar

1. **Clone o repositório**

```
bash
git clone https://github.com/Jean5316/API_AGENDA.git
cd API_AGENDA
```

2. **Restaure as dependências**

```
bash
dotnet restore
```

3. **Execute a aplicação**

```
bash
cd API
dotnet run
```

4. **Acesse a documentação interativa**

```
https://localhost:{porta}/scalar/v1
```

---

## 🧪 Executando Testes

O projeto inclui testes automatizados com MSTest.

```
bash
cd Teste
dotnet test
```

---

## 🔐 Autenticação (JWT)

A API utiliza JWT Bearer Token para proteger endpoints.

### Fluxo de Autenticação

1. Usuário envia `email` e `senha` para `/api/auth/login`
2. API valida credenciais e retorna um token JWT + refresh token
3. Frontend armazena o token e o envia no header `Authorization: Bearer {token}`
4. Quando o token expira, use `/api/auth/refresh` para obter um novo

### Endpoints de Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Registra um novo usuário |
| POST | `/api/auth/login` | Autentica e retorna tokens |
| POST | `/api/auth/refresh` | Renova o token de acesso |

### Exemplo de Login

**Request:**
```
json
POST /api/auth/login
{
  "email": "usuario@teste.com",
  "senha": "123456"
}
```

**Response:**
```
json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "def50200abc123..."
}
```

### Protegendo Endpoints

Use o atributo `[Authorize]` nos controllers:

```
csharp
[Authorize]           // Requer autenticação
[Authorize(Roles = "Admin")]  // Requer role específica
```

Sem token válido, a API retorna `401 Unauthorized`.

---

## 📌 Endpoints Principais

Todos os endpoints (exceto autenticação) exigem token JWT válido.

### Contatos (`/api/contatos`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/contatos` | Lista todos os contatos do usuário |
| GET | `/api/contatos/{id}` | Obtém contato por ID |
| GET | `/api/contatos/buscar?nome={termo}` | Busca por nome |
| GET | `/api/contatos/favoritos` | Lista favoritos |
| GET | `/api/contatos/paginacao?pagina=1&tamanhoPagina=10` | Lista paginada |
| POST | `/api/contatos` | Cria novo contato |
| PUT | `/api/contatos/AtualizarContato/{id}` | Atualiza contato |
| DELETE | `/api/contatos/DeletarContato/{id}` | Remove contato |

### Administração (`/api/admin`) — Requer Role Admin

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/admin/listar-usuarios` | Lista todos os usuários |
| POST | `/api/admin/alterar-usuario?id={id}` | Altera dados do usuário |
| DELETE | `/api/admin/deletar-usuario/{id}` | Deleta usuário |

---

## 🗺️ Roadmap

### ✅ Concluídos
- [x] Autenticação JWT
- [x] Refresh Token
- [x] Roles & Permissões
- [x] CRUD de Contatos
- [x] Busca por nome
- [x] Paginação
- [x] Área Administrativa
- [x] Relacionamento Usuário-Contato
- [x] Validações
- [x] Documentação OpenAPI/Scalar
- [x] Testes básicos

### ⏳ Próximos Passos
- [ ] Logging estruturado
- [ ] Tratamento global de erros
- [ ] Cobertura de testes
- [ ] Dockerização
- [ ] Deploy em nuvem

---

## 🔗 Integração com Frontend

O frontend em Angular utiliza:
- **HTTP Interceptor**: Adiciona automaticamente o header `Authorization`
- **AuthGuard**: Protege rotas que exigem autenticação

Repositório do frontend:
🔗 [https://github.com/Jean5316/agenda-front]

---

## 📝 Anotações Técnicas

### Regex para validação de telefone
```
csharp
@"^\(\d{2}\)\d{4,5}-\d{4}$"
// Exemplo: (11)99999-9999
```

---

## 👨‍💻 Autor

Desenvolvido por **Jean Carlo**

[![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat-square&logo=github&logoColor=white)](https://github.com/Jean5316)


---

## 📄 Licença

Este projeto está sob a licença MIT.
