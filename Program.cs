using API_AGENDA.Context;
using API_AGENDA.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//Obrigaorio para mostrar no swagger
builder.Services.AddSwaggerGen();//Obrigaorio para mostrar no swagger
builder.Services.AddControllers();//Obrigaorio para mostrar no swagger
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();// Injeção de dependência do repositório


// Add DbContext
builder.Services.AddDbContext<AgendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexaoSqlite")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();//Obrigaorio para mostrar no swagger



app.Run();


