using Core.DTOs.EstadoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using System.Text.Json;

public class EstadoService : IEstadoService
{
    private const string UrlAPI = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
    private readonly IEstadoRepository _repository;
    private readonly HttpClient _httpClient;

    public EstadoService(IEstadoRepository repository, HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
    }

    public async Task PreencherTabelaComEstadosBrasil()
    {
        var response = await _httpClient.GetAsync(UrlAPI);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            List<EstadoAPIDTO> estadosDTO = JsonSerializer.Deserialize<List<EstadoAPIDTO>>(content);
            List<Estado> estados = estadosDTO.Select(dto => new Estado
            {
                Nome = dto.nome,
                siglaEstado = dto.sigla
            }).ToList();

            await _repository.AdicionarEstadosEmMassa(estados);
        }
        else
        {
            throw new Exception($"Falha ao obter os estados. Código de status: {response.StatusCode}");
        }
    }
}
