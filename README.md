# 📌 API Agenda — ASP.NET Core + JWT

[![ .NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=flat-square&logo=entity-framework&logoColor=white)](https://docs.microsoft.com/pt-br/ef/core/)
[![JWT](https://img.shields.io/badge/JWT-black?style=flat-square&logo=JSON%20Web%20Tokens&logoColor=white)](https://jwt.io/)
[![SQLite](https://img.shields.io/badge/SQLite-003B57?style=flat-square&logo=sqlite&logoColor=white)](https://www.sqlite.org/index.html)
[![Swagger/Scalar](https://img.shields.io/badge/Scalar-8B5CF6?style=flat-square)](https://scalar.com/)
[![Testes](https://img.shields.io/badge/Testes-MSTest-004880?style=flat-square&logo=microsoft&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/core/testing/)

API REST para **gerenciamento de agenda de contatos**, desenvolvida em **ASP.NET Core 10** com autenticação via **JWT (JSON Web Token)**, **Entity Framework Core** e **SQLite**.  
O backend é consumido por um frontend em **Angular 21**, responsável pelo fluxo de autenticação, proteção de rotas e interface de gerenciamento de contatos.

---

## 📚 Sumário

- [Tecnologias utilizadas](#-tecnologias-utilizadas)
- [Features implementadas](#-features-implementadas)
- [Estrutura do projeto](#-estrutura-do-projeto)
- [Configuração](#-configuração)
- [Requisitos](#-requisitos)
- [Como executar](#️-como-executar)
- [Executando testes](#-executando-testes)
- [Autenticação JWT](#-autenticação-jwt)
- [Endpoints principais](#-endpoints-principais)
- [Roadmap](#-roadmap)
- [Integração com frontend](#-integração-com-frontend)
- [Anotações técnicas](#-anotações-técnicas)
- [Contribuição](#-contribuição)
- [Autor](#-autor)
- [Licença](#-licença)

---

## 🚀 Tecnologias utilizadas

- **ASP.NET Core 10.0**
- **C#**
- **Entity Framework Core**
- **JWT (JSON Web Token)** com Refresh Token
- **Scalar** (documentação OpenAPI / alternativa ao Swagger)
- **SQLite**
- **MSTest** (testes automatizados)
- Arquitetura **Repository Pattern** + **Service Layer**

---

## 🎯 Features implementadas

### ✅ Autenticação e autorização

- [x] Login com email e senha
- [x] Geração de JWT Token
- [x] Refresh Token para renovação de sessão
- [x] Controle de roles e permissões (`Admin`, `User`)
- [x] Hash de senha com **PBKDF2**

### ✅ Gerenciamento de contatos

- [x] CRUD completo de contatos
- [x] Relacionamento entre contatos e usuário autenticado
- [x] Busca por nome
- [x] Paginação de resultados
- [x] Marcação de contatos favoritos
- [x] Validações de dados

### ✅ Área administrativa

- [x] Listagem de usuários
- [x] Atualização de dados de usuários
- [x] Exclusão de usuários (somente `Admin`)

### ✅ Estrutura e qualidade

- [x] Repository Pattern
- [x] Service Layer
- [x] DTOs para transferência de dados
- [x] Injeção de dependência
- [x] Configuração via `appsettings.json`
- [x] Middleware de CORS para consumo pelo Angular

---

## 📁 Estrutura do projeto

```txt
API_AGENDA/
├── API/                          # Projeto principal (backend)
│   ├── Controllers/              # Controladores da API
│   │   ├── AuthController.cs      # Endpoints de autenticação
│   │   ├── ContatosController.cs  # Endpoints de contatos
│   │   └── AdminController.cs     # Endpoints administrativos
│   ├── Services/                 # Camada de serviços
│   ├── Repository/               # Repositórios (acesso a dados)
│   ├── Models/                   # Entidades do banco
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Context/                  # DbContext do EF Core
│   ├── Migrations/               # Migrações do banco
│   └── Program.cs                # Configuração da aplicação
├── Teste/                        # Projeto de testes automatizados
└── README.md                     # Documentação do projeto
```

---

## ⚙️ Configuração

### `appsettings.json`

```json
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

> ⚠️ **Importante**: altere a chave JWT (`Jwt:Key`) para um valor seguro em produção (de preferência usando variáveis de ambiente ou um provider seguro de segredos).

---

## 📋 Requisitos

- **.NET 10.0 SDK**
- **Visual Studio 2022+** ou **VS Code** (com extensões C#)
- (Opcional) **Docker** para containerização

---

## ▶️ Como executar

1. **Clonar o repositório**

```bash
git clone https://github.com/Jean5316/API_AGENDA.git
cd API_AGENDA
```

2. **Restaurar as dependências**

```bash
dotnet restore
```

3. **Aplicar migrações (se necessário)**

```bash
cd API
dotnet ef database update
```

4. **Executar a aplicação**

```bash
cd API
dotnet run
```

5. **Acessar a documentação interativa (Scalar)**

```text
https://localhost:{porta}/scalar/v1
```

---

## 🧪 Executando testes

O projeto inclui testes automatizados com **MSTest**.

```bash
cd Teste
dotnet test
```

---

## 🔐 Autenticação JWT

A API utiliza **JWT Bearer Token** para proteger os endpoints.

### Fluxo de autenticação

1. O usuário envia `email` e `senha` para `/api/auth/login`;
2. A API valida as credenciais e retorna um **JWT** + **refresh token**;
3. O frontend armazena o token e o envia no header `Authorization: Bearer {token}`;
4. Quando o token expira, o cliente usa `/api/auth/refresh` para obter um novo token de acesso.

### Endpoints de autenticação

| Método | Endpoint               | Descrição                     |
|--------|------------------------|-------------------------------|
| POST   | `/api/auth/register`   | Registra um novo usuário      |
| POST   | `/api/auth/login`      | Autentica e retorna tokens    |
| POST   | `/api/auth/refresh`    | Renova o token de acesso      |

### Exemplo de login

**Request**

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@teste.com",
  "senha": "123456"
}
```

**Response**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "def50200abc123..."
}
```

### Protegendo endpoints

Use o atributo `[Authorize]` nos controllers:

```csharp
[Authorize]                    // Requer autenticação
[Authorize(Roles = "Admin")]   // Requer role específica
```

Sem token válido, a API retorna **`401 Unauthorized`**.

---

## 📌 Endpoints principais

Todos os endpoints (exceto autenticação) exigem um token JWT válido.

### Contatos (`/api/contatos`)

| Método | Endpoint                                               | Descrição                          |
|--------|--------------------------------------------------------|------------------------------------|
| GET    | `/api/contatos`                                       | Lista todos os contatos do usuário |
| GET    | `/api/contatos/{id}`                                  | Obtém contato por ID               |
| GET    | `/api/contatos/buscar?nome={termo}`                   | Busca por nome                     |
| GET    | `/api/contatos/favoritos`                             | Lista contatos favoritos           |
| GET    | `/api/contatos/paginacao?pagina=1&tamanhoPagina=10`   | Lista paginada                     |
| POST   | `/api/contatos`                                       | Cria um novo contato               |
| PUT    | `/api/contatos/AtualizarContato/{id}`                 | Atualiza um contato existente      |
| DELETE | `/api/contatos/DeletarContato/{id}`                   | Remove um contato                  |

### Administração (`/api/admin`) — requer role `Admin`

| Método | Endpoint                                  | Descrição                 |
|--------|-------------------------------------------|---------------------------|
| GET    | `/api/admin/listar-usuarios`             | Lista todos os usuários   |
| POST   | `/api/admin/alterar-usuario?id={id}`     | Altera dados do usuário   |
| DELETE | `/api/admin/deletar-usuario/{id}`        | Exclui um usuário         |

---

## 🗺️ Roadmap

### ✅ Concluídos

- [x] Autenticação JWT
- [x] Refresh Token
- [x] Roles & Permissões
- [x] CRUD de contatos
- [x] Busca por nome
- [x] Paginação
- [x] Área administrativa
- [x] Relacionamento usuário-contato
- [x] Validações
- [x] Documentação OpenAPI / Scalar
- [x] Testes básicos

### ⏳ Próximos passos

- [ ] Logging estruturado
- [ ] Tratamento global de erros
- [ ] Aumento da cobertura de testes
- [ ] Dockerização
- [ ] Deploy em nuvem

---

## 🔗 Integração com frontend

O frontend em **Angular 21** utiliza:

- **HTTP Interceptor**: adiciona automaticamente o header `Authorization`;
- **AuthGuard**: protege rotas que exigem autenticação;
- Mecanismos de armazenamento seguro para tokens (ex.: `localStorage` ou outra estratégia configurada).

Repositório do frontend:

- [`agenda-front`](https://github.com/Jean5316/agenda-front)

---

## 📝 Anotações técnicas

### Regex para validação de telefone

```csharp
@"^\(\d{2}\)\d{4,5}-\d{4}$"
// Exemplo: (11)99999-9999
```

---

## 🤝 Contribuição

Contribuições são bem-vindas!  
Para contribuir:

1. Faça um **fork** do repositório;
2. Crie uma branch para sua feature ou correção:  
   `git checkout -b feature/minha-feature`;
3. Implemente suas alterações e adicione testes (quando aplicável);
4. Envie um **pull request** com uma descrição clara do que foi feito.

---

## 👨‍💻 Autor

Desenvolvido por **Jean Carlo**.

[![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat-square&logo=github&logoColor=white)](https://github.com/Jean5316)

---

## 📄 Licença

Este projeto está licenciado sob os termos da licença **MIT**. Consulte o arquivo `LICENSE` (se aplicável) para mais detalhes.
