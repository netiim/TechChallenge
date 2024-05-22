using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Teste.Services.Regioes
{
    public class RegiaoServiceTests
    {
        private readonly Mock<IRegiaoRepository> _regiaoRepositoryMock;
        private readonly Mock<IEstadoRepository> _estadoRepositoryMock;
        private readonly Mock<ILogger<RegiaoService>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly RegiaoService _regiaoService;

        public RegiaoServiceTests()
        {
            _regiaoRepositoryMock = new Mock<IRegiaoRepository>();
            _estadoRepositoryMock = new Mock<IEstadoRepository>();
            _loggerMock = new Mock<ILogger<RegiaoService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://brasilapi.com.br")
            };

            _regiaoService = new RegiaoService(_regiaoRepositoryMock.Object, _estadoRepositoryMock.Object, _loggerMock.Object, _httpClient);
        }

        [Fact]
        public async Task PreencherRegioesComDDD_DeveAdicionarRegioes_QuandoApiResponseForBemSucedida()
        {
            // Arrange
            var estados = new List<Estado>
        {
            new Estado { Id = 1, Nome = "São Paulo", siglaEstado = "SP" },
            new Estado { Id = 2, Nome = "Rio de Janeiro", siglaEstado = "RJ" }
        };

            _estadoRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(estados);

            var brasilApiDTO = new BrasilAPIdddDTO { state = "SP" };
            var responseContent = new StringContent(JsonSerializer.Serialize(brasilApiDTO));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = responseContent
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            _regiaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Regiao>())).Returns(Task.CompletedTask);

            // Act
            await _regiaoService.PreencherRegioesComDDD();

            // Assert
            _regiaoRepositoryMock.Verify(repo => repo.Adicionar(It.Is<Regiao>(r => r.numeroDDD == 11 && r.EstadoId == 1)), Times.AtLeastOnce);
        }

        [Fact]
        public async Task PreencherRegioesComDDD_DeveLogarWarning_QuandoApiResponseNaoForBemSucedida()
        {
            // Arrange
            var estados = new List<Estado>
        {
            new Estado { Id = 1, Nome = "São Paulo", siglaEstado = "SP" },
            new Estado { Id = 2, Nome = "Rio de Janeiro", siglaEstado = "RJ" }
        };

            _estadoRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(estados);

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            await _regiaoService.PreencherRegioesComDDD();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Falha ao obter dados para o DDD")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }
    }
}
