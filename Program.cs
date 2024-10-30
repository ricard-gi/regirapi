using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using regirapi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura la connexió a la base de dades (ajusta la cadena de connexió segons el teu entorn)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Afegir els controladors
builder.Services.AddControllers();

// Configuració de Swagger per a la documentació de l'API (opcional però recomanat)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuració de Swagger a l'entorn de desenvolupament
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configura l'API per utilitzar HTTPS
app.UseHttpsRedirection();

// Configura les rutes dels controladors
app.MapControllers();

// Executa l'aplicació
app.Run();
