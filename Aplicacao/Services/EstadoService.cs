using Core.DTOs;
using Core.Entidades;
using Core.Interfaces;
using System.Text.Json;

public class EstadoService : IEstadoService
{
    private const string UrlAPI = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
    private readonly IEstadoRepository _repository;
    public EstadoService(IEstadoRepository repository)
    {
        _repository = repository;
    }

    public async Task PreencherTabelaComEstadosBrasil()
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync(UrlAPI);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<EstadoAPIDTO> estadosDTO = JsonSerializer.Deserialize<List<EstadoAPIDTO>>(content);
                List<Estado> estados = new List<Estado>();
                foreach (EstadoAPIDTO estadoDTO in estadosDTO)
                {
                    estados.Add(new Estado() 
                    {
                        Nome = estadoDTO.nome,
                        siglaEstado = estadoDTO.sigla
                    });
                }

                await _repository.AdicionarEstadosEmMassa(estados);
            }
            else
            {
                throw new Exception($"Falha ao obter os estados. Código de status: {response.StatusCode}");
            }
        }
    }
}
