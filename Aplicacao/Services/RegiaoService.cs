using Aplicacao.Util;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class RegiaoService : IRegiaoService
{
    private const string UrlAPI = "https://brasilapi.com.br/api/ddd/v1/";
    private readonly IRegiaoRepository _regiaorepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly ILogger<RegiaoService> _logger;
    private List<Estado> Estados;

    public RegiaoService(IRegiaoRepository repository, IEstadoRepository estadoRepository, ILogger<RegiaoService> logger)
    {
        _regiaorepository = repository;
        _estadoRepository = estadoRepository;
        Estados = new List<Estado>();
        _logger = logger;
    }

    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
    {
        return await _regiaorepository.ObterTodosAsync();
    }

    public async Task PreencherRegioesComDDD()
    {
        List<int> DDDPosiveis = Enumerable.Range(11, 89).ToList();
        Estados = await _estadoRepository.GetAll();

        foreach (int ddd in DDDPosiveis)
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = await client.GetAsync($"{UrlAPI}{ddd}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    BrasilAPIdddDTO brasilApiDTO = JsonSerializer.Deserialize<BrasilAPIdddDTO>(content);

                    Regiao regiao = MontaRegiaoComRetornoAPI(ddd, brasilApiDTO);

                    await _regiaorepository.Adicionar(regiao);
                }
                else
                {
                    _logger.LogWarning($"Falha ao obter dados para o DDD {ddd}. StatusCode: {response.StatusCode}");
                }
            }
        }
    }

    private Regiao MontaRegiaoComRetornoAPI(int ddd, BrasilAPIdddDTO brasilApiDTO)
    {
        try
        {
            ValidandoRetornoAPI(brasilApiDTO);

            return new Regiao()
            {
                numeroDDD = ddd,
                EstadoId = (int)Estados.FirstOrDefault(x => x.siglaEstado.Equals(brasilApiDTO.state))?.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao montar região para o DDD {ddd}");
            throw;
        }
    }

    private void ValidandoRetornoAPI(BrasilAPIdddDTO brasilApiDTO)
    {
        ValidaNullidadeObjetoAPI(brasilApiDTO);
        ValidaSiglaExisteNaTabelaEstado(brasilApiDTO);
    }

    private void ValidaSiglaExisteNaTabelaEstado(BrasilAPIdddDTO brasilApiDTO)
    {
        if (Estados.FirstOrDefault(x => x.siglaEstado.Equals(brasilApiDTO.state)) == null)
        {
            throw new Exception("Nenhuma sigla correspondente a sigla retornada pela API");
        }
    }

    private static void ValidaNullidadeObjetoAPI(BrasilAPIdddDTO brasilApiDTO)
    {
        if (brasilApiDTO == null)
        {
            throw new Exception("Objeto API vazio");
        }
    }
}
