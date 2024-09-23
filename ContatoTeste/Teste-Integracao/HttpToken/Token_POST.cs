using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using System.Net;
using static Core.Entidades.Usuario;

namespace Testes.Integracao.HttpToken
{
    public class Token_POST : BaseIntegrationTest
    {
        public Token_POST(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task POST_Gera_Token_Usuario_Valido()
        {
            //Arrange
            Usuario newUser = new()
            {
                Username = "neto",
                Password = "123456",
                Perfil = PerfilUsuario.Administrador
            };
            try
            {
                _context.Usuario.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro: {e.Message}, {e.StackTrace}");
            }
            var user = new UsuarioTokenDTO { Username = "neto", Password = "123456" };
            using var client = app.CreateClient();

            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token/CriarToken", user);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }
        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task POST_Gera_Token_Usuario_InValido()
        {
            //Arrange
            Usuario newUser = new()
            {
                Username = "neto",
                Password = "123456",
                Perfil = PerfilUsuario.Administrador
            };
            try
            {
                _context.Usuario.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Erro: {e.Message}, {e.StackTrace}");
            }
            var user = new UsuarioTokenDTO { Username = "neto", Password = "1234567" };
            using var client = app.CreateClient();
             
            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token/CriarToken", user);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }
        [Fact]
        [Trait("Categoria", "IntegracaoContato")]
        public async Task POST_Criar_Usuario_Valido()
        {
            //Arrange
            CreateUsuarioDTO newUser = new()
            {
                Username = "neto",
                Password = "123456",
                Perfil = PerfilUsuario.Administrador
            };
            using var client = app.CreateClient();

            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token/criar-usuario", newUser);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

    }
}
