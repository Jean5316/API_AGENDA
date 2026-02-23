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
    c.SwaggerDoc("v1", new () { Title = "API Agenda", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token aqui}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});//Obrigaorio para mostrar no swagger, configurações para autenticação no swagger
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
//mostrar retorno na home
app.MapGet("/", () => new Home()).AllowAnonymous().WithTags("Home");// Rota para verificar se a API está funcionando

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");// Habilita o CORS com a política definida para o Angular
app.UseHttpsRedirection();// Redireciona HTTP para HTTPS

app.UseAuthentication();// Habilita a autenticação
app.UseAuthorization();// Habilita a autorização
app.MapControllers();//Obrigaorio para mostrar no swagger

app.Run();


