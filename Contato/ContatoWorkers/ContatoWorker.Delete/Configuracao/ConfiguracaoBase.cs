using Aplicacao.Services;
using Aplicacao.Validators;
using Core.DTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infraestrutura.Repositorios;
using MassTransit;
using ContatoWorker.Delete.Consumers;
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
    public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<DeleteContatoConsumer>()
                .Endpoint(e => e.Name = "contato-delete");

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.UsePrometheusMetrics(serviceName: "contato-delete");

                cfg.ConfigureEndpoints(context);
            });
        });
    }
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped);
    }
}
