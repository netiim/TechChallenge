using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ContatoAPI.Controllers;

namespace Testes.Controllers.Estados
{
    public class EstadoControllerTests
    {
        private readonly Mock<IEstadoService> _mockEstadoService;
        private readonly EstadoController _estadoController;

        public EstadoControllerTests()
        {
            _mockEstadoService = new Mock<IEstadoService>();
            _estadoController = new EstadoController(null, _mockEstadoService.Object);
        }

        [Fact]
        public async Task PreencherTabelaComEstadosBrasil_DeveRetornarOkResult_QuandoOperacaoBemSucedida()
        {
            // Arrange
            _mockEstadoService.Setup(service => service.PreencherTabelaComEstadosBrasil()).Returns(Task.CompletedTask);

            // Act
            var result = await _estadoController.PreencherTabelaComEstadosBrasil();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task PreencherTabelaComEstadosBrasil_DeveRetornarBadRequest_QuandoOcorreExcecao()
        {
            // Arrange
            _mockEstadoService.Setup(service => service.PreencherTabelaComEstadosBrasil()).ThrowsAsync(new Exception("Erro ao preencher tabela"));

            // Act
            var result = await _estadoController.PreencherTabelaComEstadosBrasil();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao preencher tabela", badRequestResult.Value);
        }
    }
}
