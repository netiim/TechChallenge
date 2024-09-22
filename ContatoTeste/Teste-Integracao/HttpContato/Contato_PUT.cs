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
    public class Contato_PUT : BaseIntegrationTest
    {
        public Contato_PUT(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contato_PorId_Com_Sucesso()
        {
            //Arange
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = await app.GetClientWithAccessTokenAsync();

            CreateContatoDTO createContatoDTO = new CreateContatoDTO()
            {
                Nome = "José",
                Telefone = "31995878310",
                Email = "jose@gmail.com"
            };

            //Action
            var response = await client.PutAsJsonAsync("/Contato/" + contato.Id, createContatoDTO);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contato_PorId_Informacoes_Invalidas()
        {
            //Arange
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = await app.GetClientWithAccessTokenAsync();

            CreateContatoDTO createContatoDTO = new CreateContatoDTO()
            {
                Nome = "José",
                Telefone = "10995878310",
                Email = "jose@gmail.com"
            };

            //Action
            var response = await client.PutAsJsonAsync("/Contato/" + contato.Id, createContatoDTO);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task PUT_Contato_PorId_Contato_Inexistente()
        {
            //Arange
            using var client = await app.GetClientWithAccessTokenAsync();

            CreateContatoDTO createContatoDTO = new CreateContatoDTO()
            {
                Nome = "José",
                Telefone = "10995878310",
                Email = "jose@gmail.com"
            };

            //Action
            var response = await client.PutAsJsonAsync("/Contato/" + -1, createContatoDTO);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        private Contato BuscarPrimeiroContatoDoBanco()
        {
            Contato contato = _context.Contato.OrderBy(e => e.Id).FirstOrDefault();
            if (contato is null)
            {
                Regiao regiao = _context.Regiao.OrderBy(e => e.Id).FirstOrDefault();
                if (regiao is null)
                {
                    regiao = new Regiao()
                    {
                        NumeroDDD = 31,
                        EstadoId = 15,
                        Estado = new Estado() { Nome = "Minas Gerais", siglaEstado = "MG" },
                        IdLocalidadeAPI = Guid.NewGuid().ToString()
                    };
                    _context.Regiao.Add(regiao);
                    _context.SaveChanges();

                    regiao = new Regiao()
                    {
                        NumeroDDD = 11,
                        EstadoId = 20,
                        Estado = new Estado() { Nome = "São Paulo", siglaEstado = "SP" },
                        IdLocalidadeAPI = Guid.NewGuid().ToString()
                    };
                    _context.Regiao.Add(regiao);
                    _context.SaveChanges();
                }

                contato = new Contato()
                {
                    Nome = "Paulo",
                    Email = "paulo@gmail.com",
                    Telefone = "11995878310",
                    RegiaoId = regiao.Id
                };

                _context.Contato.Add(contato);
                _context.SaveChanges();
            }

            return contato;
        }
    }
}
