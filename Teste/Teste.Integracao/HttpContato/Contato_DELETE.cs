using Core.DTOs.ContatoDTO;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpContato
{
    public class Contato_DELETE : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Contato_DELETE(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task DELETE_Contato_PorId_Com_Sucesso()
        {
            //Arange
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = await app.GetClientWithAccessTokenAsync();

            //Action
            var response = await client.DeleteAsync("/Contato/" + contato.Id);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task DELETE_Contato_PorId_Contato_Inexistente()
        {
            //Arange
            using var client = await app.GetClientWithAccessTokenAsync();

            //Action
            var response = await client.DeleteAsync("/Contato/" + -1);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        private Contato BuscarPrimeiroContatoDoBanco()
        {
            Contato contato = app.Context.Contato.OrderBy(e => e.Id).FirstOrDefault();
            if (contato is null)
            {
                Regiao regiao = app.Context.Regiao.OrderBy(e => e.Id).FirstOrDefault();
                if (regiao is null)
                {
                    regiao = new Regiao()
                    {
                        NumeroDDD = 31,
                        EstadoId = 15,
                        Estado = new Estado() { Nome = "Minas Gerais", siglaEstado = "MG" }
                    };
                    app.Context.Regiao.Add(regiao);
                    app.Context.SaveChanges();

                    regiao = new Regiao()
                    {
                        NumeroDDD = 11,
                        EstadoId = 20,
                        Estado = new Estado() { Nome = "São Paulo", siglaEstado = "SP" }
                    };
                    app.Context.Regiao.Add(regiao);
                    app.Context.SaveChanges();
                }

                contato = new Contato()
                {
                    Nome = "Paulo",
                    Email = "paulo@gmail.com",
                    Telefone = "11995878310",
                    RegiaoId = regiao.Id
                };

                app.Context.Contato.Add(contato);
                app.Context.SaveChanges();
            }

            return contato;
        }
    }
}
