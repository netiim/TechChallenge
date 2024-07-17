using Aplicacao.Validators;
using Core.Entidades;
using Core.Interfaces.Repository;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Testes.Services.Contatos.Validators
{
    public class ContatoValidatorTests
    {
        private readonly ContatoValidator _validator;
        private readonly Mock<IRegiaoRepository> _regiaoRepositoryMock;

        public ContatoValidatorTests()
        {
            _regiaoRepositoryMock = new Mock<IRegiaoRepository>();

            _regiaoRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Regiao, bool>>>()))
              .ReturnsAsync(new List<Regiao> { new Regiao { Id = 1, NumeroDDD = 11 },
                                               new Regiao { Id = 2, NumeroDDD = 21 },
                                               new Regiao { Id = 3, NumeroDDD = 31 } });

            _validator = new ContatoValidator(_regiaoRepositoryMock.Object); 
        }

        [Theory]
        [InlineData("11999999999")] 
        [InlineData("21999999999")] 
        [InlineData("31999999999")] 
        public async Task ValidarDDD_Valido_DevePassar(string telefone)
        {
            // Arrange
            Contato contato = new Contato { Nome = "Teste Nome", Email = "teste@teste.com", Telefone = telefone };

            // Act
            ValidationResult result = await _validator.ValidateAsync(contato);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("1199999999")] 
        [InlineData("119999999999")] 
        public async Task ValidarDDD_ComprimentoIncorreto_DeveFalhar(string telefone)
        {
            // Arrange
            Contato contato = new Contato { Nome = "Teste Nome", Email = "teste@teste.com", Telefone = telefone };

            // Act
            ValidationResult result = await _validator.ValidateAsync(contato);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(failure => failure.PropertyName == "Telefone" && failure.ErrorMessage == "O telefone deve ter 11 dígitos, incluindo o DDD.");
        }

        [Theory]
        [InlineData("20999999999")] 
        [InlineData("50999999999")] 
        public async Task ValidarDDD_Invalido_DeveFalhar(string telefone)
        {
            // Arrange
            Contato contato = new Contato { Nome = "Teste Nome", Email = "teste@teste.com", Telefone = telefone };
            _regiaoRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Regiao, bool>>>()))
              .ReturnsAsync(new List<Regiao> {});
            // Act
            ValidationResult result = await _validator.ValidateAsync(contato);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(failure => failure.PropertyName == "Telefone" && failure.ErrorMessage == "O DDD não corresponde a uma região válida.");
        }
    }
}
