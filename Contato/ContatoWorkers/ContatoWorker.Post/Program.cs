using ContatoAPI.Extension;
using ContatoWorker.Post.Consumers;
using FluentValidation.AspNetCore;
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
    x.AddConsumer<PostContatoConsumer>()
        .Endpoint(e => e.Name = "contato-post");

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.UsePrometheusMetrics(serviceName: "contato-post");

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();

host.Run();
