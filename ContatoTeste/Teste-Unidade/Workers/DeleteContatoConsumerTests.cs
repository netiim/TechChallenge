using Moq;
using MassTransit.Testing;
using Core.Contratos.Request;
using Core.Interfaces.Services;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Contratos.Contatos;
using Core.Contratos.Response;
using ContatoWorker.Delete.Consumers;

namespace Testes.Workers.DeleteContatoConsumers;
public class DeleteContatoConsumerTests
{
    private readonly Mock<IContatoService> _contatoServiceMock;

    public DeleteContatoConsumerTests()
    {
        _contatoServiceMock = new Mock<IContatoService>();
    }

    [Fact]
    public async Task Deve_ResponderComSucessoAposAdicionarContato()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new DeleteContatoConsumer(_contatoServiceMock.Object));

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
            var contatoDTO = new ReadContatoDTO
            {
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };

            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);

            // Configura o mock para simular a remoção com sucesso
            _contatoServiceMock.Setup(s => s.RemoverAsync(contato.Id)).Returns(Task.CompletedTask);


            // Act
            await harness.InputQueueSendEndpoint.Send(new DeleteContatoRequest
            {
                Id = 1
            });

            // Assert
            var response = harness.Published.Select<ContatoSucessResponse>().FirstOrDefault();

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
        var consumerHarness = harness.Consumer(() => new DeleteContatoConsumer(_contatoServiceMock.Object));

        await harness.Start();

        try
        {
            // Configura o mock para retornar null (contato não encontrado)
            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Act
            await harness.InputQueueSendEndpoint.Send(new DeleteContatoRequest
            {
                Id = 1  // Simule um ID de contato
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
        var consumerHarness = harness.Consumer(() => new DeleteContatoConsumer(_contatoServiceMock.Object));

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

            _contatoServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);

            // Configura o mock para simular a remoção com sucesso
            _contatoServiceMock.Setup(s => s.RemoverAsync(contato.Id)).ThrowsAsync(new Exception("Erro ao remover contato"));

            // Act
            await harness.InputQueueSendEndpoint.Send(new DeleteContatoRequest
            {
                Id = 1    // Simule um ID de contato
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
