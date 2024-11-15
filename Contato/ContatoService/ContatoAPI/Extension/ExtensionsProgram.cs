using Aplicacao.Services;
using Aplicacao.Validators;
using Core.DTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infraestrutura.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TemplateWebApiNet8.Logging;
using TemplateWebApiNet8.Services;
using MassTransit;
using Aplicacao.Consumers;
using Core.DTOs.ContatoDTO;
using Core.Contratos.Request;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Contatos.Aplicacao.Services;
using Contatos.Core.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ContatoAPI.Extension;

public static class ExtensionsProgram
{
    public static IServiceCollection AddInjecoesDependencias(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEstadoService, EstadoService>();
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<IRegiaoService, RegiaoService>();
        services.AddScoped<IRegiaoRepository, RegiaoRepository>();
        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IContatoService, ContatoService>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IDatabaseService, DatabaseService>();

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
        string? host = env.IsDevelopment() ? configuration["RabbitMq:Host"] : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        string? username = env.IsDevelopment() ? configuration["RabbitMq:Username"] : Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        string? senha = env.IsDevelopment() ? configuration["RabbitMq:Password"] : Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RegiaoConsumer>();
            x.AddConsumer<EstadoConsumer>();

            x.AddRequestClient<GetContatosRequest>(new Uri("exchange:contato-get"));
            x.AddRequestClient<PostContatosRequest>(new Uri("exchange:contato-post"));
            x.AddRequestClient<DeleteContatoRequest>(new Uri("exchange:contato-delete"));
            x.AddRequestClient<PutContatoRequest>(new Uri("exchange:contato-put"));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(senha);
                });

                cfg.UsePrometheusMetrics(serviceName: "contato_service");

                cfg.ReceiveEndpoint("regiao-queue", ep =>
                {
                    ep.ConfigureConsumer<RegiaoConsumer>(context);
                });

                cfg.ReceiveEndpoint("estado-queue", ep =>
                {
                    ep.ConfigureConsumer<EstadoConsumer>(context);
                });
            });
        });
    }
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        string? connectionString = env.IsDevelopment()
            ? configuration.GetConnectionString("DefaultConnection")
            : Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped);
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

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        byte[] key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information
        }));

        return logging;
    }

}
