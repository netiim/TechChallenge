using Core.Entidades;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Testes;

namespace LocalizacaoService.Testes.Teste.Integracao.RegiaoController
{
    public class Regiao_DELETE : BaseIntegrationTest
    {
        private readonly IMongoCollection<Regiao> _regiaoCollection;
        public Regiao_DELETE(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) 
        { 
            _regiaoCollection = _database.GetCollection<Regiao>("Regiao"); 
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task DELETE_Todas_Regioes_Sucesso()
        {
            //Assert
            var client = app.CreateClient();
            Regiao reg = new Regiao()
            {
                NumeroDDD = 12,
                Estado = new Estado()
                {
                    Nome = "Minas Gerais",
                    siglaEstado = "MG"
                }
            };
            _regiaoCollection.InsertOne(reg);

            //Action
            var response = await client.DeleteAsync("/Regiao");
   
           
            //Assert
            Assert.Equal(0, _regiaoCollection.CountDocuments(new BsonDocument()));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
