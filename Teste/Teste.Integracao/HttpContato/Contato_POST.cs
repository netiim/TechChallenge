using Core.DTOs.ContatoDTO;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpContato
{
    public class Contato_POST : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Contato_POST(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task POST_Preenche_Regioes_Sem_Autorizacao()
        {
            //Arrange
            CreateContatoDTO contato = new CreateContatoDTO()
            {
                Nome = "Paulo",
                Email = "paulo@gmail.com",
                Telefone = "11995878310",
            };
            using var client = app.CreateClient();

            //Action
            var resultado = await client.PostAsJsonAsync("/Contato", contato);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task POST_Preenche_Regioes_Com_Autorizacao()
        {
            //Arrange
            Regiao regiao = app.Context.Regiao.FirstOrDefault();
            if (regiao is null)
            {
                regiao = new Regiao()
                {
                    NumeroDDD = 11,
                    EstadoId = 20,
                    Estado = new Estado() { Nome = "São Paulo", siglaEstado = "SP" }
                };
                app.Context.Regiao.Add(regiao);
                app.Context.SaveChanges();
            }

            CreateContatoDTO contato = new CreateContatoDTO()
            {
                Nome = "Paulo",
                Email = "paulo@gmail.com",
                Telefone = $"{regiao.NumeroDDD}995878310",
            };

            using var client = await app.GetClientWithAccessTokenAsync();

            //Action
            var resultado = await client.PostAsJsonAsync("/Contato", contato);

            //Assert
            Assert.Equal(HttpStatusCode.Created, resultado.StatusCode);
        }
    }

}
