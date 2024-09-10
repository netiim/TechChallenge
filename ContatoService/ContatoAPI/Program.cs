using FluentValidation.AspNetCore;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ContatoAPI.Extension;
using Prometheus;
using MassTransit;
using Aplicacao.Consumers;

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
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RegiaoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint("regiao-queue", ep =>
        {
            ep.ConfigureConsumer<RegiaoConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

byte[] key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));

builder.Logging.AddCustomLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<RequestDelayMiddleware>(10000);

app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMetricServer();

app.Run();


public partial class Program { }