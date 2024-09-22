using AutoMapper;
using ContatoWorker.Get.Consumers;
using Core.Contratos.Contatos;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit.Testing;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Testes.Unidade.GetContatoConsumers;

public class GetContatoConsumerTests
{
    private readonly Mock<IContatoService> _contatoServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetContatoConsumerTests()
    {
        _contatoServiceMock = new Mock<IContatoService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async void Deve_ResponderComSucessoAposAdicionarContato()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new GetContatosConsumer(_contatoServiceMock.Object, _mapperMock.Object));
        await harness.Start();
        try
        {
            var contato = new Contato()
            {
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"

            };
            var contatoDTO = new List<ReadContatoDTO>()
            {
                  new ReadContatoDTO()
                  {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                  }
            };

            _mapperMock.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>())).Returns(contatoDTO);

            // Configura o mock para simular sucesso no serviço de contatos
            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);

            // Act
            await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
            {
                ContatoId = 1,
                NumeroDDD = 31
            });

            // Assert
            var response = harness.Published.Select<ContatosResponse>().FirstOrDefault();

            Assert.NotNull(response);
            Assert.NotNull(response.Context.Message.Contatos);
        }
        finally
        {
            await harness.Stop();
        }
    }
    [Fact]
    public async void Deve_ResponderComSucessoAposListarPorDDD()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new GetContatosConsumer(_contatoServiceMock.Object, _mapperMock.Object));
        await harness.Start();
        try
        {
            var contato = new List<Contato>()
            {
                new Contato()
                {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                }
            };
            var contatoDTO = new List<ReadContatoDTO>()
            {
                  new ReadContatoDTO()
                  {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                  }
            };

            _mapperMock.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>())).Returns(contatoDTO);

            // Configura o mock para simular sucesso no serviço de contatos
            _contatoServiceMock.Setup(s => s.FindAsync(It.IsAny<Expression<Func<Contato, bool>>>())).ReturnsAsync(contato);

            // Act
            await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
            {
                ContatoId = 0,
                NumeroDDD = 31
            });

            // Assert
            var response = harness.Published.Select<ContatosResponse>().FirstOrDefault();

            Assert.NotNull(response);
            Assert.NotNull(response.Context.Message.Contatos);
        }
        finally
        {
            await harness.Stop();
        }
    }
    [Fact]
    public async void Deve_ResponderComSucessoAposListarTodos()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new GetContatosConsumer(_contatoServiceMock.Object, _mapperMock.Object));
        await harness.Start();
        try
        {
            var contato = new List<Contato>()
            {
                new Contato()
                {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                }
            };
            var contatoDTO = new List<ReadContatoDTO>()
            {
                  new ReadContatoDTO()
                  {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                  }
            };

            _mapperMock.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>())).Returns(contatoDTO);

            // Configura o mock para simular sucesso no serviço de contatos
            _contatoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(contato);

            // Act
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
        finally
        {
            await harness.Stop();
        }
    }
    [Fact]
    public async void Deve_ResponderComErroAposListarTodos()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new GetContatosConsumer(_contatoServiceMock.Object, _mapperMock.Object));
        await harness.Start();
        try
        {
            var contato = new List<Contato>()
            {
                new Contato()
                {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                }
            };
            var contatoDTO = new List<ReadContatoDTO>()
            {
                  new ReadContatoDTO()
                  {
                      Nome = "paulo neto",
                      Email = "neto@gmail.com",
                      Telefone = "31995878341"
                  }
            };

            _mapperMock.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>())).Throws(new Exception("Erro de mapeamento"));

            // Configura o mock para simular sucesso no serviço de contatos
            _contatoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(contato);

            // Act
            await harness.InputQueueSendEndpoint.Send(new GetContatosRequest
            {
                ContatoId = 0,
                NumeroDDD = 0
            });

            // Assert
            var response = harness.Published.Select<ContatoErroResponse>().FirstOrDefault();

            Assert.NotNull(response);
            Assert.NotNull(response.Context.Message.MensagemErro);
        }
        finally
        {
            await harness.Stop();
        }
    }
}