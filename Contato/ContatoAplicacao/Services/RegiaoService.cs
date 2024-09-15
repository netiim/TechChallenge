using Aplicacao.Util;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class RegiaoService : IRegiaoService
{
    private readonly IRegiaoRepository _regiaorepository;

    public RegiaoService(IRegiaoRepository repository)
    {
        _regiaorepository = repository;
    }

    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
    {
        return await _regiaorepository.ObterTodosAsync();
    }
}
