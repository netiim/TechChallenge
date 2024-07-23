using System.Net;
using System.Text;
using System.Text.Json;

namespace Testes.Integracao.HttpEstado
{
    public class Estado_POST : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Estado_POST(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task POST_Preenche_Estados_Sem_Autorizacao()
        {
            //Arrange
            using var client = app.CreateClient();

            //Action
            var resultado = await client.PostAsync("/Estado", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task POST_Preenche_Estados_Com_Autorizacao()
        {
            //Arrange
            using var client = await app.GetClientWithAccessTokenAsync();
            var content = new StringContent(
                                JsonSerializer.Serialize(new {}),
                                Encoding.UTF8,
                                "application/json"
                            );

            //Action
            var resultado = await client.PostAsync("/Estado", content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

    }
}
