using Core.DTOs.UsuarioDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpToken
{
    public class Token_POST : BaseIntegrationTest
    {
        public Token_POST(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task POST_Gera_Token_Usuario_Valido()
        {
            //Arrange
            var user = new UsuarioTokenDTO { Username = "netim", Password = "123456" };
            using var client = app.CreateClient();
            
            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token", user);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }
        [Fact]
        [Trait("Categoria", "IntegraçãoGit")]
        public async Task POST_Gera_Token_Usuario_InValido()
        {
            //Arrange
            var user = new UsuarioTokenDTO { Username = "netiim", Password = "123456" };
            using var client = app.CreateClient();
             
            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token", user);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }
    }
}
