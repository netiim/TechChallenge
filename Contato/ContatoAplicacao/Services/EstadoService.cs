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

    public async Task<IEnumerable<Estado>> ObterTodos(){

        return await _repository.GetAll();
    }
}
