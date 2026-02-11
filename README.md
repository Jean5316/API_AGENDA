# 📌 API Agenda — ASP.NET Core + JWT

API REST desenvolvida em **ASP.NET Core** com autenticação via **JWT (JSON Web Token)**.  
Esta API é consumida por um frontend em **Angular 21**, responsável pelo login, proteção de rotas e gerenciamento de contatos.

---

## 🚀 Tecnologias utilizadas

- ASP.NET Core Web API
- C#
- Entity Framework Core
- JWT (JSON Web Token)
- Swagger (OpenAPI)
- SQL Server / SQLite
- Autenticação e Autorização

---

## 🎯 Objetivo do projeto

Fornecer uma API segura para:
- Autenticação de usuários
- Controle de acesso via JWT
- Gerenciamento de uma agenda de contatos
- Integração com frontend Angular

---

## 🔐 Autenticação JWT

A API utiliza **JWT Bearer Token** para proteger seus endpoints.

### Fluxo de autenticação

1. Usuário envia **email e senha**
2. API valida as credenciais
3. API gera um **JWT**
4. Token é retornado ao frontend
5. O frontend envia o token automaticamente nas requisições protegidas

---

## 🔑 Endpoint de Login

### `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "usuario@teste.com",
  "senha": "123456"
}
Response:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

🧾 Claims presentes no token
O JWT contém as seguintes informações:

name → Email do usuário

id → ID do usuário

iss → Issuer da aplicação

aud → Audience configurada

exp → Data de expiração

Essas informações são utilizadas para validação e controle de acesso.

🔒 Proteção de Endpoints
```text
Endpoints protegidos utilizam o atributo:

[Authorize]
Exemplo:

[Authorize]
[HttpGet("contatos")]
public IActionResult GetContatos()
{
    return Ok();
}
Sem token válido, a API retorna:

401 Unauthorized
```

🧩 Configuração do JWT
Configuração realizada no appsettings.json:
```json
"Jwt": {
  "Key": "CHAVE_SUPER_SECRETA_COM_MAIS_DE_32_CARACTERES",
  "Issuer": "API_AGENDA",
  "Audience": "API_AGENDA_USUARIOS"
}
E configurada no Program.cs usando AddAuthentication e AddJwtBearer.
```

```text
📂 Estrutura do projeto
API_AGENDA/
 ├── Controllers/
 │   ├── AuthController.cs
 │   └── ContatosController.cs
 ├── Entities/
 │   ├── Usuario.cs
 │   └── Contato.cs
 ├── DTOs/
 │   ├── LoginDto.cs
 │   └── ContatoDto.cs
 ├── Data/
 │   └── AppDbContext.cs
 ├── Services/
 │   └── TokenService.cs
 ├── Program.cs
 └── appsettings.json
```

🧪 Testes via Swagger
Acesse:

https://localhost:xxxx/swagger
Faça login via /api/auth/login

Copie o token retornado

Clique em Authorize

Informe:

Bearer SEU_TOKEN
Teste os endpoints protegidos

🔗 Integração com o Frontend
Esta API é consumida por um frontend desenvolvido em Angular 21, que utiliza:

Interceptor HTTP para envio automático do token

AuthGuard para proteção de rotas

Login baseado em JWT

➡️ Repositório do frontend:https://github.com/Jean5316/agenda-front

📌 Próximos passos
 CRUD completo de contatos

 Vínculo de contatos por usuário

 Refresh Token

 Roles e permissões

 Logs e tratamento global de erros

👤 Autor
Desenvolvido por Jean Carlo
💻 GitHub: https://github.com/Jean5316

