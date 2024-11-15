using Aplicacao.Services;
using Aplicacao.Validators;
using Core.DTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infraestrutura.Repositorios;
using MassTransit;
using ContatoWorker.Put.Consumers;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace ContatoAPI.Extension;

public static class ConfiguracaoBase
{
    public static IServiceCollection AddInjecoesDependencias(this IServiceCollection services)
    {
        services.AddScoped<IRegiaoService, RegiaoService>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IContatoService, ContatoService>();

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ContatoValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
    public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        string? host = env.IsDevelopment() ? configuration["RabbitMq:Host"] : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        string? username = env.IsDevelopment() ? configuration["RabbitMq:Username"] : Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        string? senha = env.IsDevelopment() ? configuration["RabbitMq:Password"] : Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<PutContatoConsumer>()
                .Endpoint(e => e.Name = "contato-put");

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(senha);
                });

                cfg.UsePrometheusMetrics(serviceName: "contato-put");

                cfg.ConfigureEndpoints(context);
            });
        });
    }
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        string? connectionString = env.IsDevelopment()
            ? configuration.GetConnectionString("DefaultConnection")
            : Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped);
    }
}
