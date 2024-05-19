using Aplicacao.Util;
using Core.DTOs;
using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using System.Collections.Generic;
using System.Text.Json;

public class CidadeService : ICidadeService
{
    private const string UrlAPI = "https://brasilapi.com.br/api/ddd/v1/";
    private readonly ICidadeRepository _cidaderepository;
    private readonly IEstadoRepository _estadoRepository;
    private List<Estado> Estados;

    public CidadeService(ICidadeRepository repository, IEstadoRepository estadoRepository)
    {
        _cidaderepository = repository;
        _estadoRepository = estadoRepository;
        Estados = new List<Estado>();
    }

    public async Task PreencherCidadesComDDD()
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

                    List<Cidade> cidades = MontaListaCidadesComRetornoAPI(ddd, brasilApiDTO);

                    await _cidaderepository.AdicionarCidadesEmMassa(cidades);
                }
                else
                {
                    //TODO:Fazer o Log para sabermos quais ddds não salvaram
                }
            }
        }
    }

    private List<Cidade> MontaListaCidadesComRetornoAPI(int ddd, BrasilAPIdddDTO brasilApiDTO)
    {
        if (brasilApiDTO == null
           || Estados.FirstOrDefault(x => x.siglaEstado.Equals(brasilApiDTO.state)) == null)
        {
            return null;
        }

        List<Cidade> cidades = MapeaBrasilAPIdddDTOParaCidade(ddd, brasilApiDTO);

        return cidades;
    }

    private List<Cidade> MapeaBrasilAPIdddDTOParaCidade(int ddd, BrasilAPIdddDTO brasilApiDTO)
    {
        List<Cidade> cidades = new List<Cidade>();

        foreach (string cidade in brasilApiDTO.cities)
        {
            cidades.Add(new Cidade()
            {
                Nome = Helper.ToCamelCase(cidade),
                numeroDDD = ddd,
                EstadoId = (int)Estados.FirstOrDefault(x => x.siglaEstado.Equals(brasilApiDTO.state))?.Id
            });
        }

        return cidades;
    }
}
