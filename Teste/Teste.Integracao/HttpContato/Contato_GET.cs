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
    public class Contato_GET : IClassFixture<TechChallengeWebApplicationFactory>
    {
        private readonly TechChallengeWebApplicationFactory app;

        public Contato_GET(TechChallengeWebApplicationFactory app)
        {
            this.app = app;
        }
        [Fact]
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
            Contato contato = app.Context.Contato.FirstOrDefault();
            if (contato is null)
            {
                Regiao regiao = app.Context.Regiao.FirstOrDefault();
                if (regiao is null)
                {
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
