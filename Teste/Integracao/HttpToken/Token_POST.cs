using Core.DTOs.UsuarioDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpToken
{
    public class Token_POST
    {
        [Fact]
        public async Task POST_Efetua_Login_Com_Sucesso()
        {
            //Arrange
            var app = new TechChallengeWebApplicationFactory();

            var user = new UsuarioTokenDTO { Username = "netim", Password = "123456" };

            using var client = app.CreateClient();
            //Action
            var resultado = await client.PostAsJsonAsync("/api/Token", user);
            //Assert
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
        }
    }
}
