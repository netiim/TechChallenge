using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Testes;

namespace LocalizacaoService.Testes.Teste.Integracao.RegiaoController
{
    public class Regiao_GET : BaseIntegrationTest
    {
        public Regiao_GET(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "IntegracaoLocalizacao")]
        public async Task GET_Obtem_Todas_Regioes_Sucesso()
        {
            var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<Regiao>>("/Regiao");

            //Assert
            Assert.NotNull(resultado);
        }
    }
}
