using Azure;
using Core.DTOs.RegiaoDTO;
using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpRegiao
{
    public class Regiao_GET : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Regiao_GET(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }
        [Fact]
        public async Task GET_Obtem_Todas_Regioes_Sucesso()
        {
            //Arrange
            Regiao regiao = app.Context.Regiao.First();
            if(regiao is null)
            {
               regiao = new Regiao()
                {
                    NumeroDDD = 11,
                    EstadoId = 20,
                    Estado = new Estado() { Id = 20, Nome = "São Paulo", siglaEstado = "SP" }
                };

                app.Context.Regiao.Add(regiao);
                app.Context.SaveChanges();
            }          
            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadRegiaoDTO>>("/Regiao");

            //Assert
            Assert.NotNull(resultado);
        }
    }
}
