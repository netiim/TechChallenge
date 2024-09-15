using ContatoAPI.Extension;
using ContatoWorker.Delete.Consumers;
using Infraestrutura.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInjecoesDependencias();
builder.Services.AddAutoMapper();
builder.Services.AddFluentValidation();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeleteContatoConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.UsePrometheusMetrics(serviceName: "contato-delete");

        cfg.ReceiveEndpoint("contato-delete-queue", ep =>
        {
            ep.ConfigureConsumer<DeleteContatoConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

var host = builder.Build();
host.Run();
