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

namespace Testes.Services.Regioes
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

            _regiaoService = new RegiaoService(_regiaoRepositoryMock.Object);
        }

    }
}
