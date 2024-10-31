using AutoMapper;
using ContatoWorker.Get.Consumers;
using Core.Contratos.Contatos;
using Core.DTOs.ContatoDTO;
using Core.Interfaces.Services;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Testes.Integracao.HttpContato
{
    public class Contato_GET : BaseIntegrationTest
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ConfiguracaoBD config;
        public Contato_GET(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            _factory = integrationTechChallengerWebAppFactory;
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Contatos_Com_Sucesso()
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
                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new GetContatosConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
                {
                    ContatoId = 0,
                    NumeroDDD = 0
                });

                // Assert
                var response = harness.Published.Select<ContatosResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.Contatos);
            }
        }
        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Contato_PorId_Com_Sucesso()
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
                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new GetContatosConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
                {
                    ContatoId = 1,
                    NumeroDDD = 0
                });

                // Assert
                var response = harness.Published.Select<ContatosResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.Contatos);
            }
        }
        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Contato_PorDDD_Com_Sucesso()
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
                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new GetContatosConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
                {
                    ContatoId = 0,
                    NumeroDDD = 11
                });

                // Assert
                var response = harness.Published.Select<ContatosResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.Contatos);
            }
        }
        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task GET_Contatos_Buscar_Nao_Encontrado()
        {
            //Arange
            using (var scope = _factory.Services.CreateScope())
            {
                //Arange
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();
                var mapper = scopedServices.GetRequiredService<IMapper>();

                // Simule a chamada HTTP POST e verifique o funcionamento do seu serviço
                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new GetContatosConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
                {
                    ContatoId = 15,
                    NumeroDDD = 31
                });

                // Assert
                var response = harness.Published.Select<ContatoNotFound>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.Mensagem);
            }
        }
    }
}
