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

var host = builder.Build();
host.Run();
