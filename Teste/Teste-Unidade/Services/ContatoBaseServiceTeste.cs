using Aplicacao.Services;
using Core.Entidades;
using Core.Interfaces.Repository;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace Testes.Services.Contatos.Base
{
    public class BaseServiceTests
    {
        private readonly Mock<IBaseRepository<Contato>> _repositoryMock;
        private readonly Mock<IValidator<Contato>> _validatorMock;
        private readonly BaseService<Contato> _service;

        public BaseServiceTests()
        {
            _repositoryMock = new Mock<IBaseRepository<Contato>>();
            _validatorMock = new Mock<IValidator<Contato>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Contato>(), default))
              .ReturnsAsync(new ValidationResult());
            _service = new BaseService<Contato>(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarContato()
        {
            // Arrange
            var contato = new Contato { Id = 1, Nome = "Teste 1", Telefone="31995878310", Email="Netim@gmail.com" };

            // Act
            await _service.AdicionarAsync(contato);

            // Assert
            _repositoryMock.Verify(repo => repo.AdicionarAsync(contato), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarContato()
        {
            // Arrange
            var contato = new Contato { Id = 1, Nome = "Teste 1" };

            // Act
            await _service.AtualizarAsync(contato);

            // Assert
            _repositoryMock.Verify(repo => repo.AtualizarAsync(contato), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverContatoPorId()
        {
            // Arrange
            var contatoId = 1;

            // Act
            await _service.RemoverAsync(contatoId);

            // Assert
            _repositoryMock.Verify(repo => repo.RemoverAsync(contatoId), Times.Once);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodosOsContatos()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "Teste 1" }, new Contato { Id = 2, Nome = "Teste 2" } };
            _repositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(contatos);

            // Act
            var resultado = await _service.ObterTodosAsync();

            // Assert
            resultado.Should().BeEquivalentTo(contatos);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarContatoPorId()
        {
            // Arrange
            var contatoId = 1;
            var contato = new Contato { Id = contatoId, Nome = "Teste" };
            _repositoryMock.Setup(repo => repo.ObterPorIdAsync(contatoId)).ReturnsAsync(contato);

            // Act
            var resultado = await _service.ObterPorIdAsync(contatoId);

            // Assert
            resultado.Should().BeEquivalentTo(contato);
        }

        [Fact]
        public async Task FindAsync_DeveRetornarContatosComBaseNoPredicate()
        {
            // Arrange
            Expression<Func<Contato, bool>> predicate = c => c.Nome.StartsWith("A");
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "Ana" }, new Contato { Id = 2, Nome = "Alberto" } };
            _repositoryMock.Setup(repo => repo.FindAsync(predicate)).ReturnsAsync(contatos);

            // Act
            var resultado = await _service.FindAsync(predicate);

            // Assert
            resultado.Should().BeEquivalentTo(contatos);
        }

    }
}
