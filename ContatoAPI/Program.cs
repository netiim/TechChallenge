using Core.Interfaces;
using Infraestrutura.Data;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddScoped<IEstadoService,EstadoService>();
builder.Services.AddScoped<IEstadoRepository,EstadoRepository>();
builder.Services.AddScoped<ICidadeService, CidadeService>();
builder.Services.AddScoped<ICidadeRepository,CidadeRepository>();

var connectionString = configuration.GetValue<string>("ConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
