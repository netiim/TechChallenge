using Xunit;
using Moq;
using FluentAssertions;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Aplicacao.Services;
using Core.Entidades;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Testes.Services.Contatos
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly Mock<IValidator<Contato>> _validatorMock;
        private readonly Mock<IRegiaoRepository> _regiaoRepositoryMock;
        private readonly ContatoService _contatoService;

        public ContatoServiceTests()
        {
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _validatorMock = new Mock<IValidator<Contato>>();
            _regiaoRepositoryMock = new Mock<IRegiaoRepository>();

            _regiaoRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Regiao, bool>>>()))
                .ReturnsAsync(new List<Regiao> { new Regiao { Id = 1, NumeroDDD = 11 } });

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Contato>(), default))
                .ReturnsAsync(new ValidationResult());

            _contatoService = new ContatoService(_contatoRepositoryMock.Object, _validatorMock.Object, _regiaoRepositoryMock.Object);
        }

        [Fact]
        [Trait("Categoria", "Unidade")]
        public async Task ObterTodosAsync_DeveRetornarTodosOsContatos()
        {
            // Arrange
            var expectedContatos = new List<Contato>
            {
                new Contato { Id = 1, Nome = "Contato 1", Email = "contato1@test.com", Telefone = "11999999999" },
                new Contato { Id = 2, Nome = "Contato 2", Email = "contato2@test.com", Telefone = "21999999999" }
            };
            _contatoRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(expectedContatos);

            // Act
            var result = await _contatoService.ObterTodosAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedContatos);
        }
        [Fact]
        [Trait("Categoria", "Unidade")]
        public async Task AdicionarAsync_DeveLancarValidationException_QuandoTelefoneContemLetras()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste Nome",Email = "teste@teste.com", Telefone = "11A9999999B"
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(contato.Telefone), "O telefone deve conter apenas números.")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock.Setup(v => v.ValidateAsync(contato, default)).ReturnsAsync(validationResult);

            // Act
            Func<Task> act = async () => await _contatoService.AdicionarAsync(contato);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*O telefone deve conter apenas números*");
        }
        [Fact]
        [Trait("Categoria", "Unidade")]
        public async Task AdicionarAsync_DeveLancarValidationException_QuandoTelefoneEstiverVazio()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste Nome",
                Email = "teste@teste.com",
                Telefone = ""
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(contato.Telefone), "O telefone não pode ser vazio.")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock.Setup(v => v.ValidateAsync(contato, default)).ReturnsAsync(validationResult);

            // Act
            Func<Task> act = async () => await _contatoService.AdicionarAsync(contato);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*O telefone não pode ser vazio*");
        }

        [Fact]
        [Trait("Categoria", "Unidade")]
        public async Task AdicionarAsync_DeveChamarAddNoRepositorio()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste Nome",
                Email = "teste@teste.com",
                Telefone = "11999999999"
            };

            // Act
            await _contatoService.AdicionarAsync(contato);

            // Assert
            _contatoRepositoryMock.Verify(repo => repo.AdicionarAsync(contato), Times.Once);
        }

        [Fact]
        [Trait("Categoria", "Unidade")]
        public async Task ObterPorIdAsync_DeveRetornarContato()
        {
            // Arrange
            var contatoId = 1;
            var expectedContato = new Contato { Id = contatoId, Nome = "Contato 1", Email = "contato1@test.com", Telefone = "11999999999" };
            _contatoRepositoryMock.Setup(repo => repo.ObterPorIdAsync(contatoId)).ReturnsAsync(expectedContato);

            // Act
            var result = await _contatoService.ObterPorIdAsync(contatoId);

            // Assert
            result.Should().Be(expectedContato);
        }
        [Theory]
        [Trait("Categoria", "Unidade")]
        [InlineData(11)]
        [InlineData(21)]
        [InlineData(31)]
        public async Task ObterPorDdd_DeveRetornarContatosComODddInformado(int ddd)
        {
            // Arrange
            var expectedContatos = new List<Contato>
            {
                new Contato { Id = 1, Nome = "Contato 1", Email = "contato1@test.com", Telefone = $"{ddd}999999999" },
                new Contato { Id = 2, Nome = "Contato 2", Email = "contato2@test.com", Telefone = $"{ddd}888888888" }
            };
            _contatoRepositoryMock.Setup(repo => repo.FindAsync(c => c.Regiao.NumeroDDD == ddd)).ReturnsAsync(expectedContatos);

            // Act
            var result = await _contatoService.FindAsync(c => c.Regiao.NumeroDDD == ddd);

            // Assert
            Assert.Equal(expectedContatos.Count, result.Count());
            Assert.All(result, contato => Assert.StartsWith(ddd.ToString(), contato.Telefone));
        }        
    }
}
