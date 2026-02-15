using API_AGENDA.Context;
using API_AGENDA.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//Obrigaorio para mostrar no swagger, habilita a exploração de endpoints para o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new () { Title = "API Agenda", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Digite: Bearer {seu token aqui}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});//Obrigaorio para mostrar no swagger, configurações para autenticação no swagger

builder.Services.AddControllers();//Obrigaorio para mostrar no swagger
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();// Injeção de dependência do repositório


var jwtSettings = builder.Configuration.GetSection("Jwt");// Configurações do JWT
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);// Chave secreta para assinatura do token

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
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});// Configurações de autenticação JWT

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowCredentials()
               .AllowAnyHeader();
    });
});// Configurações de CORS para permitir requisições do Angular


// Add DbContext, configurando para usar SQLite com a string de conexão definida no appsettings.json
builder.Services.AddDbContext<AgendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexaoSqlite")));

var app = builder.Build();

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


