using Azure;
using Core.DTOs.ContatoDTO;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Integracao.HttpContato
{
    public class Contato_GET : BaseIntegrationTest
    {
        public Contato_GET(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory)
            : base(integrationTechChallengerWebAppFactory) { }

        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task GET_Obtem_Todos_Contatos_Com_Sucesso()
        {
            // Verifica se existe um contato
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadContatoDTO>>("/Contato/BuscarTodosContatos");

            //Assert
            Assert.NotNull(resultado);
        }      
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task GET_Obtem_Contatos_Por_DDD_Com_Sucesso()
        {
            // Verifica se existe um contato
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<List<ReadContatoDTO>>("/Contato/BuscarPorDDD?ddd=" + contato.Regiao.NumeroDDD);

            //Assert
            Assert.NotNull(resultado);

        }     
        [Fact]
        [Trait("Categoria", "Integração")]
        public async Task GET_Obtem_Contatos_Por_Id_Com_Sucesso()
        {
            // Verifica se existe um contato
            Contato contato = BuscarPrimeiroContatoDoBanco();
            using var client = app.CreateClient();

            //Action
            var resultado = await client.GetFromJsonAsync<ReadContatoDTO>("/Contato/" + contato.Id);

            //Assert
            Assert.NotNull(resultado);
            Assert.Equal(contato.Nome, resultado.Nome);
            Assert.Equal(contato.Telefone, resultado.Telefone);
            Assert.Equal(contato.Email, resultado.Email);
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
