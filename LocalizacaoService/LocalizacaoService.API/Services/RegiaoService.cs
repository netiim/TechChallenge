using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using LocalizacaoService.Interfaces.Repository;
using LocalizacaoService.Interfaces.Services;
using LocalizacaoService.Interfaces.Validators;
using MappingRabbitMq.Models;
using MassTransit;
using System.Text.Json;

public class RegiaoService : IRegiaoService
{
    private const string UrlAPI = "https://brasilapi.com.br/api/ddd/v1/";
    private readonly IRegiaoRepository _regiaorepository;
    private readonly IEstadoService _estadoService;
    private readonly IRegiaoValidator _regiaoValidator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RegiaoService> _logger;
    private readonly HttpClient _httpClient;
    private List<ReadEstadoDTO> Estados;

    public RegiaoService(
        IRegiaoRepository repository,
        IEstadoService estadoService,
        IRegiaoValidator regiaoValidator,
        ILogger<RegiaoService> logger,
        HttpClient httpClient,
        IPublishEndpoint publishEndpoint)
    {
        _regiaorepository = repository;
        _estadoService = estadoService;
        _regiaoValidator = regiaoValidator;
        _logger = logger;
        _httpClient = httpClient;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
    {
        return await _regiaorepository.GetAllAsync();
    }
    public async Task RemoverRegioesAsync()
    {
        await _regiaorepository.DeleteManyAsync();
    }
    public async Task CadastrarRegioesAsync()
    {
        try
        {
            Estados = await _estadoService.BuscarEstadosBrasilAsync();
            await PublicarEstados();
            await CriarRegioes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao montar as regiões");
            throw new Exception(ex.Message);
        }
    }

    private async Task PublicarEstados()
    {
        foreach (var estado in Estados)
        {
            await _publishEndpoint.Publish(estado);
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
                _logger.LogInformation($"Publicando mensagem da região: {regiao.Estado}");

                ReadEstadoDTO estado = new ReadEstadoDTO()
                {
                    Nome = regiao.Estado.Nome,
                    siglaEstado = regiao.Estado.siglaEstado
                };

                RegiaoConsumerDTO reg = new RegiaoConsumerDTO()
                {
                    Id = regiao.Id,
                    NumeroDDD = regiao.NumeroDDD,
                    DataCriacao = regiao.DataCriacao,
                    siglaEstado = regiao.Estado.siglaEstado
                };
                await _publishEndpoint.Publish(reg);
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
            ReadEstadoDTO readEstado = Estados.FirstOrDefault(x => x.siglaEstado.Equals(regiaoApiDTO.state));
            return new Regiao()
            {
                NumeroDDD = ddd,
                Estado = new Estado()
                {
                    Nome = readEstado.Nome,
                    siglaEstado = readEstado.siglaEstado
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao montar região para o DDD {ddd}");
            throw new Exception(ex.Message);
        }
    }
}
