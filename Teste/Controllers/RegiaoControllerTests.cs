using AutoMapper;
using Core.DTOs.RegiaoDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ContatoAPI.Controllers;
using Core.Entidades;

namespace Teste.Controllers.Regioes
{
    public class RegiaoControllerTests
    {
        private readonly Mock<IRegiaoService> _mockRegiaoService;
        private readonly Mock<ILogger<RegiaoController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RegiaoController _regiaoController;

        public RegiaoControllerTests()
        {
            _mockRegiaoService = new Mock<IRegiaoService>();
            _mockLogger = new Mock<ILogger<RegiaoController>>();
            _mockMapper = new Mock<IMapper>();
            _regiaoController = new RegiaoController(_mockRegiaoService.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task PreencherCidadesComDDD_DeveRetornarOkResult_QuandoOperacaoBemSucedida()
        {
            // Arrange
            _mockRegiaoService.Setup(service => service.PreencherRegioesComDDD()).Returns(Task.CompletedTask);

            // Act
            var result = await _regiaoController.PreencherCidadesComDDD();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task PreencherCidadesComDDD_DeveRetornarBadRequest_QuandoOcorreExcecao()
        {
            // Arrange
            _mockRegiaoService.Setup(service => service.PreencherRegioesComDDD()).ThrowsAsync(new Exception("Erro ao preencher regiões"));

            // Act
            var result = await _regiaoController.PreencherCidadesComDDD();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao preencher regiões", badRequestResult.Value);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarOkResultComListaDeRegiaoDTO_QuandoOperacaoBemSucedida()
        {
            // Arrange
            var regioes = new List<Regiao>
            {
                new Regiao { Id = 1, numeroDDD = 11 },
                new Regiao { Id = 2, numeroDDD = 21 }
            };
            var regiaoDTOs = new List<ReadRegiaoDTO>
            {
                new ReadRegiaoDTO { numeroDDD = 11 },
                new ReadRegiaoDTO { numeroDDD = 21 }
            };

            _mockRegiaoService.Setup(service => service.ObterTodosAsync()).ReturnsAsync(regioes);
            _mockMapper.Setup(mapper => mapper.Map<List<ReadRegiaoDTO>>(regioes)).Returns(regiaoDTOs);

            // Act
            var result = await _regiaoController.ObterTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRegiaoDTOs = Assert.IsType<List<ReadRegiaoDTO>>(okResult.Value);
            Assert.Equal(2, returnedRegiaoDTOs.Count);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarBadRequest_QuandoOcorreExcecao()
        {
            // Arrange
            _mockRegiaoService.Setup(service => service.ObterTodosAsync()).ThrowsAsync(new Exception("Erro ao obter regiões"));

            // Act
            var result = await _regiaoController.ObterTodos();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao obter regiões", badRequestResult.Value);
        }
    }
}
