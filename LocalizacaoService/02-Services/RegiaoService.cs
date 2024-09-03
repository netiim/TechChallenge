using Core.DTOs.EstadoDTO;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using LocalizacaoService.Interfaces.Repository;
using LocalizacaoService.Interfaces.Services;
using System.Text.Json;

public class RegiaoService : IRegiaoService
{
    private const string UrlAPI = "https://brasilapi.com.br/api/ddd/v1/";
    private const string UrlAPIEstado = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
    private readonly IRegiaoRepository _regiaorepository;
    private readonly ILogger<RegiaoService> _logger;
    private List<Estado> Estados;
    private readonly HttpClient _httpClient;
    public RegiaoService(IRegiaoRepository repository, ILogger<RegiaoService> logger, HttpClient httpClient)
    {
        _regiaorepository = repository;
        Estados = new List<Estado>();
        _logger = logger;
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
    {
        return await _regiaorepository.GetAllAsync();
    }
    public async Task CadastrarRegioesAsync()
    {
        try
        {
            await CriarRegioes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao montar as regiões");
            throw new Exception(ex.Message);
        }
    }
    private async Task CriarRegioes()
    {
        Estados = await BuscarEstadosBrasil();
        List<int> DDDPosiveis = Enumerable.Range(11, 89).ToList();
        foreach (int ddd in DDDPosiveis)
        {
            var response = await _httpClient.GetAsync($"{UrlAPI}{ddd}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Core.DTOs.RegiaoDTO.RegiaoAPIDTO brasilApiDTO = JsonSerializer.Deserialize<Core.DTOs.RegiaoDTO.RegiaoAPIDTO>(content);

                Regiao regiao = MontaRegiaoComRetornoAPI(ddd, brasilApiDTO);
                await InserirRegiaoBancoDados(regiao);
            }
            else
            {
                _logger.LogWarning($"Falha ao obter dados para o DDD {ddd}. StatusCode: {response.StatusCode}");
            }
        }
    }
    private async Task InserirRegiaoBancoDados(Regiao regiao)
    {
        if (await _regiaorepository.GetByDDDAsync(regiao.NumeroDDD) == null)
        {
            await _regiaorepository.CreateAsync(regiao);
        }        
    }
    private async Task<List<Estado>> BuscarEstadosBrasil()
    {
        var response = await _httpClient.GetAsync(UrlAPIEstado);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            List<EstadoAPIDTO> estadosDTO = JsonSerializer.Deserialize<List<EstadoAPIDTO>>(content);
            List<Estado> estados = estadosDTO.Select(dto => new Estado
            {
                Nome = dto.nome,
                siglaEstado = dto.sigla
            }).ToList();

            return estados;
        }
        else
        {
            throw new Exception($"Falha ao obter os estados. Código de status: {response.StatusCode}");
        }
    }
    private Regiao MontaRegiaoComRetornoAPI(int ddd, RegiaoAPIDTO regiaoApiDTO)
    {
        try
        {
            ValidandoRetornoAPI(regiaoApiDTO);

            return new Regiao()
            {
                NumeroDDD = ddd,
                Estado = Estados.FirstOrDefault(x => x.siglaEstado.Equals(regiaoApiDTO.state))
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao montar região para o DDD {ddd}");
            throw new Exception(ex.Message);
        }
    }
    private void ValidandoRetornoAPI(RegiaoAPIDTO regiaoApiDTO)
    {
        ValidaNullidadeObjetoAPI(regiaoApiDTO);
        ValidaSiglaExisteNaTabelaEstado(regiaoApiDTO);
    }
    private void ValidaSiglaExisteNaTabelaEstado(RegiaoAPIDTO regiaoApiDTO)
    {
        if (Estados.FirstOrDefault(x => x.siglaEstado.Equals(regiaoApiDTO.state)) == null)
        {
            throw new Exception("Nenhuma sigla correspondente a sigla retornada pela API");
        }
    }
    private static void ValidaNullidadeObjetoAPI(RegiaoAPIDTO regiaoApiDTO)
    {
        if (regiaoApiDTO == null)
        {
            throw new Exception("Objeto API vazio");
        }
    }
}
