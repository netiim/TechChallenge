using Infraestrutura.Repositorios;
using LocalizacaoService;
using LocalizacaoService._02_Services;
using LocalizacaoService._03_Repositorys.Config;
using LocalizacaoService.Interfaces.Repository;
using LocalizacaoService.Interfaces.Services;
using LocalizacaoService.Interfaces.Validators;
using LocalizacaoService.Validators;
using MappingRabbitMq.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;
using System.Text;

namespace ContatoAPI.Extension;

public static class ExtensionsProgram
{
    public static IServiceCollection AddInjecoesDependencias(this IServiceCollection services)
    {
        services.AddScoped<IRegiaoService, RegiaoService>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IEstadoService, EstadoService>();
        services.AddScoped<IRegiaoValidator, RegiaoValidator>();

        return services;
    }

    public static IServiceCollection AddDocumentacaoSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech Challenge API" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(xmlPath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization Header -  utilizado com Bearer Authentication" +
                              "Digite 'Bearer' [espaço] token" +
                              "Exemplo (informar sem as aspas): 'Bearer 123456abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
    public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        string? host = env.IsDevelopment() 
            ? configuration["RabbitMq:Host"] 
            : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        string? username = env.IsDevelopment() 
            ? configuration["RabbitMq:Username"] 
            : Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        string? senha = env.IsDevelopment() 
            ? configuration["RabbitMq:Password"] 
            : Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(senha);
                });

                cfg.UsePrometheusMetrics(serviceName: "localizacao_service");

                cfg.Message<RegiaoConsumerDTO>(configTopology => { });
                cfg.Message<ReadEstadoDTO>(configTopology => { });
            });
        });
    }
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        var mongoSettings = new MongoDbSettings
        {
            ConnectionString = env.IsDevelopment()
                ? configuration["MongoDbSettings:ConnectionString"]
                : Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"),
            DatabaseName = env.IsDevelopment()
                ? configuration["MongoDbSettings:DatabaseName"]
                : Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")
        };

        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = mongoSettings.ConnectionString;
            options.DatabaseName = mongoSettings.DatabaseName;
        });

        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });
    }
}
