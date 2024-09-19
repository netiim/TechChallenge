﻿using Aplicacao.Services;
using Aplicacao.Validators;
using Core.DTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infraestrutura.Repositorios;

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
}