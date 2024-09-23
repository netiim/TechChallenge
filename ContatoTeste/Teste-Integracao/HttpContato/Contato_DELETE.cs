using AutoMapper;
using ContatoWorker.Delete.Consumers;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.Contratos.Response;
using Core.Interfaces.Services;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Testes.Integracao.HttpContato
{
    public class Contato_DELETE : BaseIntegrationTest
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ConfiguracaoBD config;
        public Contato_DELETE(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            _factory = integrationTechChallengerWebAppFactory;
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato-Delete")]
        public async Task DELETE_Contatos_Com_Sucesso()
        {
            //Arange
            using (var scope = _factory.Services.CreateScope())
            {
                //Arange
                config.AdicionarContatoAoBancodDados();
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();
                var mapper = scopedServices.GetRequiredService<IMapper>();

                // Simule a chamada HTTP POST e verifique o funcionamento do seu serviço
                using var client = await app.GetClientWithAccessTokenAsync();

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new DeleteContatoConsumer(contatoService));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new DeleteContatoRequest
                {
                    Id = 1
                });

                // Assert
                var response = harness.Published.Select<ContatoSucessResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message);
            }
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato-Delete")]
        public async Task DELETE_Contato_Nao_Encontrado()
        {
            //Arange
            using (var scope = _factory.Services.CreateScope())
            {
                //Arange
                config.AdicionarContatoAoBancodDados();
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();
                var mapper = scopedServices.GetRequiredService<IMapper>();

                // Simule a chamada HTTP POST e verifique o funcionamento do seu serviço
                using var client = await app.GetClientWithAccessTokenAsync();

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new DeleteContatoConsumer(contatoService));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new DeleteContatoRequest
                {
                    Id = 0
                });

                // Assert
                var response = harness.Published.Select<ContatoNotFound>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message);
            }
        }

    }
}
