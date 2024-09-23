using Moq;
using MassTransit.Testing;
using ContatoWorker.Put.Consumers;
using Core.Contratos.Request;
using Core.Interfaces.Services;
using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Contratos.Contatos;
using Core.Contratos.Response;

namespace Testes.Workers.PutContatoConsumers;
public class PutContatoConsumerTests
{
    private readonly Mock<IContatoService> _contatoServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public PutContatoConsumerTests()
    {
        _contatoServiceMock = new Mock<IContatoService>();
        _mapperMock = new Mock<IMapper>();
    }
    [Fact]
    public async Task Deve_ResponderComSucessoAposAdicionarContato()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new PutContatoConsumer(_contatoServiceMock.Object, _mapperMock.Object));

        await harness.Start();

        try
        {
            var contato = new Contato
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            var contatoDTO = new PutContatoDTO
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);

            // Configura o mock para simular a remoção com sucesso
            _contatoServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<Contato>())).Returns(Task.CompletedTask);

            // Act
            await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
            {
                ContatoDTO = contatoDTO    // Simule um ID de contato
            });
            // Assert
            var response = harness.Published.Select<ContatoResponse>().FirstOrDefault();

            Assert.NotNull(response);
            Assert.NotNull(response.Context.Message);
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Deve_RetornarContatoNotFound_QuandoContatoNaoExistir()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new PutContatoConsumer(_contatoServiceMock.Object, _mapperMock.Object));

        await harness.Start();

        try
        {
            var contato = new Contato
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            var contatoDTO = new PutContatoDTO
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Configura o mock para simular a remoção com sucesso
            _contatoServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<Contato>())).Returns(Task.CompletedTask);

            // Act
            await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
            {
                ContatoDTO = contatoDTO    // Simule um ID de contato
            });
            // Assert
            var response = harness.Published.Select<ContatoNotFound>().FirstOrDefault();
            Assert.NotNull(response);  // Verifica se uma resposta foi publicada
            Assert.Equal("Contato não foi encontrado no banco de dados", response.Context.Message.Mensagem);  // Verifica a mensagem de erro
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Deve_RetornarErro_QuandoOcorreExcecaoNoProcessamento()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new PutContatoConsumer(_contatoServiceMock.Object, _mapperMock.Object));

        await harness.Start();

        try
        {
            var contato = new Contato
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            var contatoDTO = new PutContatoDTO
            {
                Id = 1,
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);

            // Configura o mock para simular a remoção com sucesso
            _contatoServiceMock.Setup(s => s.AtualizarAsync(contato)).ThrowsAsync(new Exception("Erro ao remover contato"));

            // Act
            await harness.InputQueueSendEndpoint.Send(new PutContatoRequest
            {
                ContatoDTO = contatoDTO    // Simule um ID de contato
            });

            // Assert
            var response = harness.Published.Select<ContatoErroResponse>().FirstOrDefault();
            Assert.NotNull(response);  // Verifica se uma resposta foi publicada
            Assert.Contains("Erro ao remover contato", response.Context.Message.MensagemErro);  // Verifica a mensagem de erro
        }
        finally
        {
            await harness.Stop();
        }
    }
}
