using AutoMapper;
using ContatoWorker.Put.Consumers;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.Contratos.Response;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpContato
{
    public class Contato_PUT : BaseIntegrationTest
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ConfiguracaoBD config;
        public Contato_PUT(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            _factory = integrationTechChallengerWebAppFactory;
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contatos_Com_Sucesso()
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

                var contatoDTO = new PutContatoDTO
                {
                    Id = 1,
                    Nome = "paulo neto",
                    Email = "neto@gmail.com",
                    Telefone = "31995878341"
                };

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new PutContatoConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
                {
                    ContatoDTO = contatoDTO
                });

                // Assert
                var response = harness.Published.Select<ContatoResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message);
            }
        }
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contatos_Nao_Econtrado()
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

                var contatoDTO = new PutContatoDTO
                {
                    Id = 3,
                    Nome = "paulo neto",
                    Email = "neto@gmail.com",
                    Telefone = "31995878341"
                };

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new PutContatoConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
                {
                    ContatoDTO = contatoDTO
                });

                // Assert
                var response = harness.Published.Select<ContatoNotFound>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message);
            }
        }
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contatos_Com_Erro()
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

                var contatoDTO = new PutContatoDTO
                {
                    Id = 1,
                    Nome = "paulo neto",
                    Email = "neto@gmail.com",
                    Telefone = "20995878341"
                };

                var serviceProvider = app.Services;

                InMemoryTestHarness harness = new InMemoryTestHarness();
                var consumerHarness = harness.Consumer(() => new PutContatoConsumer(contatoService, mapper));

                await harness.Start();
                //Action
                await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
                {
                    ContatoDTO = contatoDTO
                });

                // Assert
                var response = harness.Published.Select<ContatoErroResponse>().FirstOrDefault();

                Assert.NotNull(response);
                Assert.NotNull(response.Context.Message);
            }
        }
    }
}
