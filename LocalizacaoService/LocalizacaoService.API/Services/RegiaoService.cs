using Core.DTOs.EstadoDTO;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using LocalizacaoService.Interfaces.Repository;
using LocalizacaoService.Interfaces.Services;
using LocalizacaoService.Interfaces.Validators;
using System.Text.Json;

public class RegiaoService : IRegiaoService
{
    private const string UrlAPI = "https://brasilapi.com.br/api/ddd/v1/";
    private readonly IRegiaoRepository _regiaorepository;
    private readonly IEstadoService _estadoService;
    private readonly IRegiaoValidator _regiaoValidator;
    private readonly ILogger<RegiaoService> _logger;
    private readonly HttpClient _httpClient;
    private List<Estado> Estados;

    public RegiaoService(
        IRegiaoRepository repository,
        IEstadoService estadoService,
        IRegiaoValidator regiaoValidator,
        ILogger<RegiaoService> logger,
        HttpClient httpClient)
    {
        _regiaorepository = repository;
        _estadoService = estadoService;
        _regiaoValidator = regiaoValidator;
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
            Estados = await _estadoService.BuscarEstadosBrasilAsync();
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
        List<int> DDDPosiveis = Enumerable.Range(11, 89).ToList();
        foreach (int ddd in DDDPosiveis)
        {
            if (await _regiaorepository.GetByDDDAsync(ddd) != null)
            {
                continue;
            }

            var response = await _httpClient.GetAsync($"{UrlAPI}{ddd}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                RegiaoAPIDTO brasilApiDTO = JsonSerializer.Deserialize<RegiaoAPIDTO>(content);

                Regiao regiao = MontaRegiaoComRetornoAPI(ddd, brasilApiDTO);
                await _regiaorepository.CreateAsync(regiao);
            }
            else
            {
                _logger.LogWarning($"Falha ao obter dados para o DDD {ddd}. StatusCode: {response.StatusCode}");
            }
        }
    }
    private Regiao MontaRegiaoComRetornoAPI(int ddd, RegiaoAPIDTO regiaoApiDTO)
    {
        try
        {
            _regiaoValidator.Validar(regiaoApiDTO, Estados);

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
}
