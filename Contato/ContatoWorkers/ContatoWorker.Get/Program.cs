using ContatoAPI.Extension;
using ContatoWorker.Get.Consumers;
using FluentValidation.AspNetCore;
using Infraestrutura.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInjecoesDependencias();
builder.Services.AddAutoMapper();
builder.Services.AddFluentValidation();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// Configuração do middleware para expor métricas
builder.WebHost.Configure(app =>
{
    app.UseRouting();
    app.UseHttpMetrics(); // Middleware para capturar métricas HTTP
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics(); // Endpoint /metrics para Prometheus
    });
});

var host = builder.Build();
host.Run();
