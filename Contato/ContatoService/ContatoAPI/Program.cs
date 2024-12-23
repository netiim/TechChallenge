using FluentValidation.AspNetCore;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ContatoAPI.Extension;
using Prometheus;
using MassTransit;
using Aplicacao.Consumers;
using Core.DTOs.ContatoDTO;
using Core.Contratos.Request;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDocumentacaoSwagger();
builder.Services.AddHttpClient();
builder.Services.AddInjecoesDependencias();
builder.Services.AddAutoMapper();
builder.Services.AddFluentValidation();
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();
var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();

builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, builder.Environment);

byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretJWT"));

builder.Logging.AddCustomLogging();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMetricServer();

app.Run();


public partial class Program { }