using ContatoAPI.Extension;
using LocalizacaoService._03_Repositorys.Config;
using MappingRabbitMq.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddInjecoesDependencias(); 

builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Localização API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMetricServer();

app.Run();

public partial class Program { }