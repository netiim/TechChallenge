using Core.DTOs.ContatoDTO;
using Core.DTOs.EstadoDTO;
using Core.Entidades;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Testes.Integracao.HttpContato;

namespace Testes.Integracao.HttpEstado
{
    public class Estado_GET : BaseIntegrationTest
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ConfiguracaoBD config;
        public Estado_GET(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            _factory = integrationTechChallengerWebAppFactory;
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Buscar_Estados_Sem_Autorizacao()
        {
            //Arrange
            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadEstadoDTO>>("/Estado");

            //Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Buscar_Estados_Com_Autorizacao()
        {
            //Arrange
            using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());
            Estado estado = new Estado() { Nome = "São Paulo", siglaEstado = "SP" };
            await _context.Estado.AddAsync(estado);
            await _context.SaveChangesAsync();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadEstadoDTO>>("/Estado");
            bool listaPossuiItens = resultado?.Count > 0;

            //Assert
            Assert.True(listaPossuiItens);
            Assert.NotNull(resultado);
        }

    }
}
