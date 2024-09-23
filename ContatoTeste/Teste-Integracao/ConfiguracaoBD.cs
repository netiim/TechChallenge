using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Testes.Integracao.HttpContato
{
    public class ConfiguracaoBD:BaseIntegrationTest
    {
        private IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory;

        public ConfiguracaoBD(IntegrationTechChallengerWebAppFactory integrationTechChallengerWebAppFactory):base(integrationTechChallengerWebAppFactory)
        {
            this.integrationTechChallengerWebAppFactory = integrationTechChallengerWebAppFactory;
        }
        public Contato AdicionarContatoAoBancodDados()
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