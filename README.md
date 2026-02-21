# 📌 API Agenda — ASP.NET Core + JWT

API REST desenvolvida em **ASP.NET Core** com autenticação via **JWT (JSON Web Token)**.
Esta API é consumida por um frontend em **Angular 21**, responsável pelo login, proteção de rotas e gerenciamento de contatos.

---

## 🚀 Tecnologias utilizadas

- ASP.NET Core (.NET 10)
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

## Requisitos

- .NET 10 SDK
- (Opcional) SQL Server ou SQLite
- (Opcional) Docker para containerização

---

## Como executar

1. Clone o repositório

```bash
git clone https://github.com/Jean5316/API_AGENDA.git
cd API_AGENDA
```

2. Atualize as configurações em `appsettings.json` (connection string, chave JWT)

3. Aplicar migrações (se usar EF Core e banco):

```bash
dotnet tool install --global dotnet-ef # se não tiver o ef tool
dotnet ef database update
```

4. Executar a API:

```bash
dotnet run
```

O Swagger normalmente ficará disponível em `https://localhost:{porta}/swagger`.

---

## 🔐 Autenticação (JWT)

A API utiliza JWT Bearer Token para proteger endpoints.

### Fluxo resumido

1. Usuário envia `email` e `senha` para o endpoint de login.
2. API valida credenciais e retorna um token JWT.
3. Frontend armazena o token e o envia no header `Authorization: Bearer {token}` nas chamadas protegidas.

### Endpoint de autenticação (exemplo)

`POST /api/auth/login`

Request Body (exemplo):

```json
{
  "email": "usuario@teste.com",
  "senha": "123456"
}
```

Response (exemplo):

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Protegendo endpoints

Use o atributo `[Authorize]` nos controllers ou actions que devem exigir autenticação.
Sem token válido, a API retorna `401 Unauthorized`.

### Configuração básica do JWT

No `appsettings.json` coloque a configuração do JWT, por exemplo:

```json
"Jwt": {
  "Key": "CHAVE_SUPER_SECRETA_COM_MAIS_DE_32_CARACTERES",
  "Issuer": "API_AGENDA",
  "Audience": "API_AGENDA_USUARIOS"
}
```

E registre a autenticação em `Program.cs` usando `AddAuthentication` e `AddJwtBearer`.

---

## Estrutura sugerida do projeto

```
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

---

## Testes e documentação (Swagger)

Ao rodar a aplicação, acesse `/swagger` para testar endpoints. Para endpoints protegidos:

1. Chame o endpoint de login e copie o token retornado.
2. Clique em `Authorize` no Swagger e cole `Bearer {token}`.
3. Teste os endpoints protegidos.

---

## Endpoints principais

Abaixo uma visão geral dos endpoints mais importantes desta API. Todos os endpoints (exceto os de autenticação) exigem um token JWT válido via header `Authorization: Bearer {token}`.

- Autenticação
  - `POST /api/auth/login` — autentica usuário e retorna um JWT.
    - Body exemplo:
      ```json
      { "email": "usuario@teste.com", "senha": "123456" }
      ```

- Contatos (`ContatosController`)
  - `GET /api/contatos` — lista todos os contatos do usuário (ativos).
  - `GET /api/contatos/{id}` — obtém um contato por ID (do usuário autenticado).
  - `GET /api/contatos/favoritos` — lista apenas contatos marcados como favoritos.
  - `GET /api/contatos/buscar?nome={nome}` — busca contatos cujo nome contenha o termo informado.
    - Exemplo: `/api/contatos/buscar?nome=jean`
  - `GET /api/contatos/paginacao?pagina={n}&tamanhoPagina={m}` — listagem paginada. Parâmetros opcionais: `pagina` (padrão 1) e `tamanhoPagina` (padrão 2).
  - `POST /api/contatos` — cria um novo contato.
    - Body: objeto `ContatoCriarDto` (nome, telefone, email, etc.).
  - `PUT /api/contatos/AtualizarContato/{id}` — atualiza um contato existente por ID.
    - Body: objeto `ContatoAtualizarDto`.
  - `DELETE /api/contatos/DeletarContato/{id}` — remove (ou marca como removido) um contato por ID.

Observação: os nomes exatos dos DTOs e propriedades seguem a implementação do projeto. Use o Swagger para ver exemplos e esquemas dos bodies.

---

## Integração com frontend

O frontend em Angular utiliza um `HTTP Interceptor` para adicionar automaticamente o header `Authorization` nas requisições e `AuthGuard` para proteger rotas.

Repositório do frontend (exemplo):
https://github.com/Jean5316/agenda-front

---

## Próximos passos / Roadmap

- Buscar por nome (search)
- Paginação
- Implementar área administrativa
- Refresh Token
- Roles & Permissões
- Tratamento global de erros e logs
- Testes automatizados
- Dockerização
- Deploy

---

## Autor
Desenvolvido por Jean Carlo

GitHub: https://github.com/Jean5316

