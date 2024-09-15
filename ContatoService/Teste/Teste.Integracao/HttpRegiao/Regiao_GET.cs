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
            //Arrange
            Estado estado = new Estado() { Nome = "São Paulo", siglaEstado = "SP" };
            await _context.Estado.AddAsync(estado);
            await _context.SaveChangesAsync();

            estado = _context.Estado.FirstOrDefault();
            if (estado is not null)
            {
                Regiao regiao = new Regiao()
                {
                    NumeroDDD = 11,
                    EstadoId = estado.Id,
                    Estado = estado,
                    IdLocalidadeAPI = Guid.NewGuid().ToString()
                };

                await _context.Regiao.AddAsync(regiao);
                await _context.SaveChangesAsync();
            }

            var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadRegiaoDTO>>("/Regiao");

            //Assert
            Assert.NotNull(resultado);
        }
    }
}
