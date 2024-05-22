using Core.DTOs.UsuarioDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TemplateWebApiNet8.Controllers;

namespace Teste.Controllers.Tokens
{
    public class TokenControllerTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILogger<TokenController>> _mockLogger;
        private readonly TokenController _tokenController;

        public TokenControllerTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<TokenController>>();
            _tokenController = new TokenController(_mockTokenService.Object, _mockLogger.Object);
        }

        [Fact]
        public void CriaToken_DeveRetornarOkResultComToken_QuandoCredenciaisValidas()
        {
            // Arrange
            var usuario = new UsuarioTokenDTO
            {
                Username = "netim",
                Password = "123456"
            };
            var token = "valid-token";
            _mockTokenService.Setup(service => service.GetToken(usuario)).Returns(token);

            // Act
            var result = _tokenController.CriaToken(usuario);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(token, okResult.Value);
        }

        [Fact]
        public void CriaToken_DeveRetornarUnauthorizedResult_QuandoCredenciaisInvalidas()
        {
            // Arrange
            var usuario = new UsuarioTokenDTO
            {
                Username = "invaliduser",
                Password = "invalidpassword"
            };
            var exceptionMessage = "Invalid credentials";
            _mockTokenService.Setup(service => service.GetToken(usuario)).Throws(new Exception(exceptionMessage));

            // Act
            var result = _tokenController.CriaToken(usuario);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(exceptionMessage, unauthorizedResult.Value);
        }
    }
}
