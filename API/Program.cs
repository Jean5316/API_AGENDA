using API_AGENDA.Context;
using API_AGENDA.Models;
using API_AGENDA.ModelViews;
using API_AGENDA.Repository;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//Obrigaorio para mostrar no swagger, habilita a exploração de endpoints para o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Agenda", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token aqui}"
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();// Adiciona o filtro de operação para verificar a autorização nos endpoints
});//Obrigaorio para mostrar no swagger, botao Authorize para autenticação JWT no Swagger
builder.Services.AddControllers();//Obrigaorio para mostrar no swagger

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();// Injeção de dependência do repositório
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();//Injeção de dependencia do PasswordHasher
builder.Services.AddScoped<IContatoService, ContatoService>();//Injeção de dependencia do contato service
builder.Services.AddScoped<ItokenService, TokenService>();//Injeção de dependencia do token service

//Configurando autenticação JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");// busca configurações no appsettings
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);// Chave secreta para assinatura do token

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
    };
});// Configurações de autenticação JWT

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyOrigin()
               .AllowAnyHeader();
    });
});// Configurações de CORS para permitir requisições do Angular


// Add DbContext, configurando para usar SQLite com a string de conexão definida no appsettings.json
builder.Services.AddDbContext<AgendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexaoSqlite")));

var app = builder.Build();
//mostrar html na raiz da aplicação para verificar se a API está funcionando, sem necessidade de autenticação
//porem nao esta funcionando por que no if de desenvolvimento o swagger esta configurado para ser mostrado na raiz da aplicação, entao o html nao aparece.
// app.MapGet("/", () =>
// {
//     var html = """
// <html>
//         <head>
//             <title>API Agenda</title>
//             <style>
//                 body {
//                     font-family: Arial, sans-serif;
//                     background-color: #f4f4f4;
//                     display: flex;
//                     justify-content: center;
//                     align-items: center;
//                     height: 100vh;
//                     margin: 0;
//                 }
//                 .container {
//                     background-color: #fff;
//                     padding: 20px 40px;
//                     border-radius: 8px;
//                     box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
//                     text-align: center;
//                 }
//                 h1 {
//                     color: #333;
//                 }
//                 p {
//                     color: #666;
//                 }
//             </style>
//         </head>
//         <body>
//             <div class="container">
//                 <h1>API Agenda</h1>
// <p>Bem-vindo à API Agenda! Esta API foi desenvolvida para
// gerenciar contatos de forma eficiente e segura. Com recursos
// de autenticação JWT, você pode criar, ler, atualizar e excluir
// contatos, garantindo que suas informações estejam protegidas.</p>
// <p>Para começar, acesse a documentação interativa da API no Swagger:</p>
// <a href="/swagger/index.html">Documentação Swagger</a>
//             </div>
//         </body>
// """;
//     return Results.Content(html, "text/html", Encoding.UTF8);
// }).AllowAnonymous().WithTags("Home");// Rota para verificar se a API está funcionando

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Agenda v1");
        c.RoutePrefix = string.Empty; // Define o Swagger UI na raiz da aplicação
    });
}

app.UseCors("Angular");// Habilita o CORS com a política definida para o Angular
app.UseHttpsRedirection();// Redireciona HTTP para HTTPS

app.UseAuthentication();// Habilita a autenticação
app.UseAuthorization();// Habilita a autorização
app.MapControllers();//Obrigaorio para mostrar no swagger

app.Run();


