using ContatoAPI.Controllers;
using Core.DTOs.EstadoDTO;
using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
        [Trait("Categoria", "Integração")]
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
        [Trait("Categoria", "Integração")]
        public async Task POST_Preenche_Regioes_Com_Autorizacao()
        {
            //Arrange
            using var client = await app.GetClientWithAccessTokenAsync();

            var content = new StringContent(
                                JsonSerializer.Serialize(new { }),
                                Encoding.UTF8,
            "application/json"
                            );
            try
            {
                List<Estado> list = ObterEstadosDoBrasil();
                app.Context.Estado.AddRange(list);
                await app.Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro: {e.Message}, {e.StackTrace}");
            }


            //Action
            var resultado = await client.PostAsync("/Regiao", null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }
        private static List<Estado> ObterEstadosDoBrasil()
        {
            return new List<Estado>
                {
                    new Estado { Nome = "Acre", siglaEstado = "AC" },
                    new Estado { Nome = "Alagoas", siglaEstado = "AL" },
                    new Estado { Nome = "Amapá", siglaEstado = "AP" },
                    new Estado { Nome = "Amazonas", siglaEstado = "AM" },
                    new Estado { Nome = "Bahia", siglaEstado = "BA" },
                    new Estado { Nome = "Ceará", siglaEstado = "CE" },
                    new Estado { Nome = "Distrito Federal", siglaEstado = "DF" },
                    new Estado { Nome = "Espírito Santo", siglaEstado = "ES" },
                    new Estado { Nome = "Goiás", siglaEstado = "GO" },
                    new Estado { Nome = "Maranhão", siglaEstado = "MA" },
                    new Estado { Nome = "Mato Grosso", siglaEstado = "MT" },
                    new Estado { Nome = "Mato Grosso do Sul", siglaEstado = "MS" },
                    new Estado { Nome = "Minas Gerais", siglaEstado = "MG" },
                    new Estado { Nome = "Pará", siglaEstado = "PA" },
                    new Estado { Nome = "Paraíba", siglaEstado = "PB" },
                    new Estado { Nome = "Paraná", siglaEstado = "PR" },
                    new Estado { Nome = "Pernambuco", siglaEstado = "PE" },
                    new Estado { Nome = "Piauí", siglaEstado = "PI" },
                    new Estado { Nome = "Rio de Janeiro", siglaEstado = "RJ" },
                    new Estado { Nome = "Rio Grande do Norte", siglaEstado = "RN" },
                    new Estado { Nome = "Rio Grande do Sul", siglaEstado = "RS" },
                    new Estado { Nome = "Rondônia", siglaEstado = "RO" },
                    new Estado { Nome = "Roraima", siglaEstado = "RR" },
                    new Estado { Nome = "Santa Catarina", siglaEstado = "SC" },
                    new Estado { Nome = "São Paulo", siglaEstado = "SP" },
                    new Estado { Nome = "Sergipe", siglaEstado = "SE" },
                    new Estado { Nome = "Tocantins", siglaEstado = "TO" }
                };
        }
    }
}
