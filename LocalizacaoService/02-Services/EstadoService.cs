using Core.DTOs.EstadoDTO;
using Core.Entidades;
using LocalizacaoService.Interfaces.Services;
using System.Text.Json;

namespace LocalizacaoService._02_Services;

public class EstadoService : IEstadoService
{
    private readonly HttpClient _httpClient;
    private const string UrlAPIEstado = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";

    public EstadoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Estado>> BuscarEstadosBrasilAsync()
    {
        var response = await _httpClient.GetAsync(UrlAPIEstado);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            List<EstadoAPIDTO> estadosDTO = JsonSerializer.Deserialize<List<EstadoAPIDTO>>(content);
            return estadosDTO.Select(dto => new Estado
            {
                Nome = dto.nome,
                siglaEstado = dto.sigla
            }).ToList();
        }
        else
        {
            throw new Exception($"Falha ao obter os estados. Código de status: {response.StatusCode}");
        }
    }
}
