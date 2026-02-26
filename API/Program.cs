using API.Repository;
using API.Repository.Interfaces;
using API.Services;
using API.Services.Interfaces;
using API_AGENDA.Context;
using API_AGENDA.Models;
using API_AGENDA.Repository;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//Obrigaorio para mostrar no swagger, habilita a exploração de endpoints para o Swagger
builder.Services.AddOpenApi(options =>
{
    // 1. Define o esquema de segurança no documento (Cria a definição do Cadeado)
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes?.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Introduza o seu Token JWT para aceder aos endpoints protegidos."
        });
        return Task.CompletedTask;
    });

    // 2. Aplica o cadeado apenas nos métodos que têm o atributo [Authorize]
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        if (metadata.Any(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute ||
                         m is Microsoft.AspNetCore.Authorization.IAuthorizeData))
        {
            operation.Security =  new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                }

            };
        }
        return Task.CompletedTask;
    });
});
builder.Services.AddControllers();//Obrigaorio para mostrar no swagger

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()



    // Log geral
    .WriteTo.File($"Logs/{DateTime.Now.ToString("yyyy-MM-dd")}/geral-.txt",
        rollingInterval: RollingInterval.Day, shared: true,
        outputTemplate:
         "{Timestamp:yyyy-MM-dd HH:mm:ss} | [{Level}] | {SourceContext} | {Message:lj}{NewLine}{Exception}")

    // Log apenas AdminController
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e =>
            e.Properties.ContainsKey("SourceContext") &&
            e.Properties["SourceContext"].ToString().Contains("AdminController"))
        .WriteTo.File($"Logs/{DateTime.Now.ToString("yyyy-MM-dd")}/admin.txt",
            rollingInterval: RollingInterval.Day, shared: true,
            outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss} | [{Level}] | {SourceContext} | {Message:lj}{NewLine}{Exception}"))

    // Log apenas AuthController
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e =>
            e.Properties.ContainsKey("SourceContext") &&
            e.Properties["SourceContext"].ToString().Contains("AuthController"))
        .WriteTo.File($"Logs/{DateTime.Now.ToString("yyyy-MM-dd")}/auth.txt",
            rollingInterval: RollingInterval.Day, shared: true,
            outputTemplate:
         "{Timestamp:yyyy-MM-dd HH:mm:ss} | [{Level}] | {SourceContext} | {Message:lj}{NewLine}{Exception}"))

    .CreateLogger();

builder.Host.UseSerilog();// Configura o host para usar o Serilog como provedor de logging

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();// Injeção de dependência do repositório
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();//Injeção de dependencia do PasswordHasher
builder.Services.AddScoped<IContatoService, ContatoService>();//Injeção de dependencia do contato service
builder.Services.AddScoped<ItokenService, TokenService>();//Injeção de dependencia do token service
builder.Services.AddScoped<IAdminService, AdminService>();//Injeção de dependencia do admin service
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();//Injeção de dependencia do usuario repository

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

    app.MapOpenApi();

    app.MapScalarApiReference("", o =>

    {

        o.WithTitle("API Agenda")

         .WithTheme(ScalarTheme.DeepSpace)

         .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)// Configura o HttpClient para a linguagem C# (pode ser ajustado para outras linguagens, se necessário)
         .WithOpenApiRoutePattern("/openapi/v1.json");//direciona o scalar para home e indica qual arquivo de documentação.


    });



}

app.UseCors("Angular");// Habilita o CORS com a política definida para o Angular
app.UseHttpsRedirection();// Redireciona HTTP para HTTPS
app.UseSerilogRequestLogging();// Habilita o middleware de logging de requisições do Serilog

app.UseAuthentication();// Habilita a autenticação
app.UseAuthorization();// Habilita a autorização
app.MapControllers();//Obrigaorio para mostrar no swagger

app.Run();


