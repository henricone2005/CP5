

using Microsoft.EntityFrameworkCore;
using PacientesePlanos.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext para usar Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionando serviços ao contêiner
builder.Services.AddControllers();

// Adicionando suporte para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurando o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
