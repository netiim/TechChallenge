using Core.DTOs.EstadoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Testes.Services.Estados;

public class EstadoServiceTests
{
    private readonly Mock<IEstadoRepository> _repositoryMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly EstadoService _estadoService;

    public EstadoServiceTests()
    {
        _repositoryMock = new Mock<IEstadoRepository>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://servicodados.ibge.gov.br")
        };

        _estadoService = new EstadoService(_repositoryMock.Object, _httpClient);
    }

    [Fact]
    [Trait("Categoria", "Unidade")]
    public async Task PreencherTabelaComEstadosBrasil_DeveAdicionarEstados_QuandoApiResponseForBemSucedida()
    {
        // Arrange
       // var estadosDTO = new List<EstadoAPIDTO>
       // {
       //     new EstadoAPIDTO { nome = "São Paulo", sigla = "SP" },
       //     new EstadoAPIDTO { nome = "Rio de Janeiro", sigla = "RJ" }
       // };
       // var responseContent = new StringContent(JsonSerializer.Serialize(estadosDTO));
       // var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
       // {
       //     Content = responseContent
       // };

       // _httpMessageHandlerMock.Protected()
       //     .Setup<Task<HttpResponseMessage>>(
       //         "SendAsync",
       //         ItExpr.IsAny<HttpRequestMessage>(),
       //         ItExpr.IsAny<CancellationToken>())
       //     .ReturnsAsync(responseMessage);

       //// _repositoryMock.Setup(repo => repo.AdicionarEstadosEmMassa(It.IsAny<List<Estado>>()))
       //    // .Returns(Task.CompletedTask);

       // // Act
       // await _estadoService.PreencherTabelaComEstadosBrasil();

       // // Assert
       // //_repositoryMock.Verify(repo => repo.AdicionarEstadosEmMassa(It.Is<List<Estado>>(estados =>
       //  //   estados.Count == 2 &&
       //   //  estados.Any(e => e.Nome == "São Paulo" && e.siglaEstado == "SP") &&
       //  //   estados.Any(e => e.Nome == "Rio de Janeiro" && e.siglaEstado == "RJ")
       // )), Times.Once);
    }

    [Fact]
    [Trait("Categoria", "Unidade")]
    public async Task PreencherTabelaComEstadosBrasil_DeveLancarExcecao_QuandoApiResponseNaoForBemSucedida()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        Func<Task> act = async () => await _estadoService.PreencherTabelaComEstadosBrasil();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Falha ao obter os estados. Código de status: BadRequest");
    }
}