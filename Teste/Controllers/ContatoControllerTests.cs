using AutoMapper;
using ContatoAPI.Controllers;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace Testes.Controllers.Contatos
{
    public class ContatoControllerTests
    {
        private readonly Mock<IContatoService> _mockContatoService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _contatoController;

        public ContatoControllerTests()
        {
            _mockContatoService = new Mock<IContatoService>();
            _mockMapper = new Mock<IMapper>();
            _contatoController = new ContatoController(null, _mockContatoService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarOkResult_ComListaDeContatos()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato(), new Contato() };
            _mockContatoService.Setup(service => service.ObterTodosAsync()).ReturnsAsync(contatos);
            _mockMapper.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>()))
                       .Returns(new List<ReadContatoDTO> { new ReadContatoDTO(), new ReadContatoDTO() });

            // Act
            var result = await _contatoController.ObterTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReadContatoDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task ObterPorDdd_DeveRetornarOkResult_ComListaDeContatosFiltrados()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Regiao = new Regiao { numeroDDD = 11 } } };
            _mockContatoService.Setup(service => service.FindAsync(It.IsAny<Expression<Func<Contato, bool>>>()))
                               .ReturnsAsync(contatos);
            _mockMapper.Setup(m => m.Map<List<ReadContatoDTO>>(It.IsAny<List<Contato>>()))
                       .Returns(new List<ReadContatoDTO> { new ReadContatoDTO() });

            // Act
            var result = await _contatoController.ObterPorDdd(11);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReadContatoDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarOkResult_QuandoContatoExiste()
        {
            // Arrange
            var contato = new Contato();
            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contato);
            _mockMapper.Setup(m => m.Map<ReadContatoDTO>(It.IsAny<Contato>())).Returns(new ReadContatoDTO());

            // Act
            var result = await _contatoController.ObterPorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReadContatoDTO>(okResult.Value);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNotFound_QuandoContatoNaoExiste()
        {
            // Arrange
            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Act
            var result = await _contatoController.ObterPorId(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarCreatedAtAction_QuandoContatoForCriado()
        {
            // Arrange
            var createContatoDTO = new CreateContatoDTO();
            var contato = new Contato();
            var readContatoDTO = new ReadContatoDTO();

            _mockMapper.Setup(m => m.Map<Contato>(It.IsAny<CreateContatoDTO>())).Returns(contato);
            _mockContatoService.Setup(service => service.AdicionarAsync(contato)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<ReadContatoDTO>(It.IsAny<Contato>())).Returns(readContatoDTO);

            // Act
            var result = await _contatoController.Adicionar(createContatoDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_contatoController.ObterPorId), createdAtActionResult.ActionName);
            Assert.IsType<ReadContatoDTO>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContent_QuandoContatoForAtualizado()
        {
            // Arrange
            var createContatoDTO = new CreateContatoDTO { Nome = "Nome Atualizado", Email = "email@teste.com", Telefone = "11987654321" };
            var contatoExistente = new Contato();

            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contatoExistente);
            _mockContatoService.Setup(service => service.AtualizarAsync(contatoExistente)).Returns(Task.CompletedTask);

            // Act
            var result = await _contatoController.Atualizar(1, createContatoDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNotFound_QuandoContatoNaoExiste()
        {
            // Arrange
            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Act
            var result = await _contatoController.Atualizar(1, new CreateContatoDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Remover_DeveRetornarNoContent_QuandoContatoForRemovido()
        {
            // Arrange
            var contatoExistente = new Contato();

            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(contatoExistente);
            _mockContatoService.Setup(service => service.RemoverAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _contatoController.Remover(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Remover_DeveRetornarNotFound_QuandoContatoNaoExiste()
        {
            // Arrange
            _mockContatoService.Setup(service => service.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Act
            var result = await _contatoController.Remover(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}