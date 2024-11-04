using AutoMapper;
using ContatoWorker.Post.Consumers;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.DTOs.ContatoDTO;
using Core.Interfaces.Services;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Testes.Integracao.HttpContato
{
    public class Contato_POST : BaseIntegrationTest
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ConfiguracaoBD config;
        public Contato_POST(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            _factory = integrationTechChallengerWebAppFactory;
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task POST_Contato_Com_Sucesso()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                //Arange
                config.AdicionarContatoAoBancodDados();
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();
                var mapper = scopedServices.GetRequiredService<IMapper>();

                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;
                var contatoDTO = new CreateContatoDTO
                {
                    Nome = "paulo neto",
                    Email = "neto@gmail.com",
                    Telefone = "31995878341"
                };


                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new PostContatoConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new PostContatosRequest
                {
                    CreateContatoDTO = contatoDTO
                });

                // Assert
                var response = harness.Published.Select<ContatoResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.Contato);
            }
        }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task POST_Contato_Com_ErroDDDInvalido()
        {            
            using (var scope = _factory.Services.CreateScope())
            {
                //Arange
                var scopedServices = scope.ServiceProvider;
                var contatoService = scopedServices.GetRequiredService<IContatoService>();
                var mapper = scopedServices.GetRequiredService<IMapper>();

                using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

                var serviceProvider = app.Services;
                var contatoDTO = new CreateContatoDTO
                {
                    Nome = "paulo neto",
                    Email = "neto@gmail.com",
                    Telefone = "20995878341"
                };


                var harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new PostContatoConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new PostContatosRequest
                {
                    CreateContatoDTO = contatoDTO
                });

                // Assert
                var response = harness.Published.Select<ContatoErroResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message.MensagemErro);
            }
        }
        
    }

}
