using Core.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Testes;

namespace LocalizacaoService.Testes.Teste.Integracao.RegiaoController;

public class Regiao_POST : BaseIntegrationTest
{
    private readonly IMongoCollection<Regiao> _regiaoCollection;
    public Regiao_POST(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
        : base(integrationTechChallengerWebAppFactory)
    {
        _regiaoCollection = _database.GetCollection<Regiao>("Regiao");
    }

    [Fact]
    [Trait("Categoria", "Integração")]
    public async Task POST_Preenhcer_Todas_Regioes_Sucesso()
    {
        //Assert
        var client = app.CreateClient();
        var content = new StringContent(
                                JsonSerializer.Serialize(new { }),
                                Encoding.UTF8,
                                "application/json"
                                );
        //Action
        var response = await client.PostAsJsonAsync("/Regiao", content);

        //Assert
        Assert.True(_regiaoCollection.CountDocuments(new BsonDocument()) > 0);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}

