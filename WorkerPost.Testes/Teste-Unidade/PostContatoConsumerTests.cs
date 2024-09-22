using Moq;
using MassTransit;
using MassTransit.Testing;
using ContatoWorker.Post.Consumers;
using Core.Contratos.Request;
using Core.Interfaces.Services;
using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Contratos.Contatos;

namespace Testes.Unidade.PostContatoConsumers;
public class PostContatoConsumerTests
{
    private readonly Mock<IContatoService> _contatoServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public PostContatoConsumerTests()
    {
        _contatoServiceMock = new Mock<IContatoService>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task Deve_ResponderComSucessoAposAdicionarContato()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new PostContatoConsumer(_contatoServiceMock.Object, _mapperMock.Object));

        await harness.Start();

        try
        {
            var contatoDTO = new CreateContatoDTO
            {
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };
            var contato = new ReadContatoDTO
            {
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };

            _mapperMock.Setup(m => m.Map<ReadContatoDTO>(It.IsAny<Contato>())).Returns(contato);

            // Configura o mock para simular sucesso no serviço de contatos
            _contatoServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<Contato>())).Returns(Task.CompletedTask);

            // Act
            await harness.InputQueueSendEndpoint.Send(new PostContatosRequest
            {
                CreateContatoDTO = contatoDTO
            });

            // Assert
            var response = harness.Published.Select<ContatoResponse>().FirstOrDefault();

            Assert.NotNull(response);
            Assert.NotNull(response.Context.Message.Contato);
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Deve_ResponderComErroQuandoFalhaAcontece()
    {
        // Arrange
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new PostContatoConsumer(_contatoServiceMock.Object, _mapperMock.Object));

        await harness.Start();

        try
        {
            var contatoDTO = new CreateContatoDTO
            {
                Nome = "paulo neto",
                Email = "neto@gmail.com",
                Telefone = "31995878341"
            };

            _mapperMock.Setup(m => m.Map<Contato>(It.IsAny<CreateContatoDTO>())).Throws(new Exception("Erro de mapeamento"));

            // Act
            await harness.InputQueueSendEndpoint.Send(new PostContatosRequest
            {
                CreateContatoDTO = contatoDTO
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
