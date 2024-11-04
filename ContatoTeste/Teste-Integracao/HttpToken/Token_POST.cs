using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Testes.Integracao.HttpContato;
using static Core.Entidades.Usuario;

namespace Testes.Integracao.HttpToken
{
    public class Token_POST : BaseIntegrationTest
    {
        private readonly ConfiguracaoBD config;
        public Token_POST(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory)
        {
            config = new ConfiguracaoBD(integrationTechChallengerWebAppFactory);
        }

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

            using var client = await app.GetClientWithAccessTokenAsync(config.AdicionarUsuarioAoBancodDados());

            CreateUsuarioDTO newUser = new()
            {
                Username = "neto",
                Password = "123456",
                Perfil = PerfilUsuario.Administrador
            };
            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token/criar-usuario", newUser);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }

    }
}
