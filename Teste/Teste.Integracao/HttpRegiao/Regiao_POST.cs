using ContatoAPI.Controllers;
using Core.DTOs.UsuarioDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpRegiao
{
    public class Regiao_POST : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Regiao_POST(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task POST_Preenche_Regioes_Sem_Autorizacao()
        {
            //Arrange
            using var client = app.CreateClient();

            //Action
            var resultado = await client.PostAsync("/Regiao", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        public async Task POST_Preenche_Regioes_Com_Autorizacao()
        {
            //Arrange
            using var client = await app.GetClientWithAccessTokenAsync();
            var content = new StringContent(
                                JsonSerializer.Serialize(new { }),
                                Encoding.UTF8,
                                "application/json"
                            );
            await client.PostAsync("/Estado", content);

            //Action
            var resultado = await client.PostAsync("/Regiao", content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }
    }
}
