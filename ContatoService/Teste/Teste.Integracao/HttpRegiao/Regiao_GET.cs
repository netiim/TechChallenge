using Azure;
using Core.DTOs.RegiaoDTO;
using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using Infraestrutura.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpRegiao
{
    public class Regiao_GET : BaseIntegrationTest
    {
        public Regiao_GET(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task GET_Obtem_Todas_Regioes_Sucesso()
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            // Verifique se as regiões foram inseridas
            Regiao regiao = _context.Regiao.OrderBy(e => e.Id).FirstOrDefault();
            if (regiao is null)
            {
                regiao = new Regiao()
                {
                    NumeroDDD = 11,
                    EstadoId = 20,
                    Estado = new Estado() {Nome = "São Paulo", siglaEstado = "SP" }
                };

                _context.Regiao.Add(regiao);
                _context.SaveChanges();
            }

            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadRegiaoDTO>>("/Regiao");

            //Assert
            Assert.NotNull(resultado);
        }
    }
}
