using Infraestrutura.Data;

namespace Testes
{
    public class DatabaseConnectionTests
    {
        private readonly ApplicationDbContext _context;

        public DatabaseConnectionTests()
        {
            var factory = new TechChallengeWebApplicationFactory();
            _context = factory.Context;
        }

    //    [Fact]
    //    [Trait("Categoria", "VerificacaoDatabaseParaTesteIntegracao")]
    //    public void VerifyConnectionString()
    //    {
    //        // Verifique a string de conexão
    //        var connectionString = _context.GetConnectionString();
    //        Assert.Contains("TestTechChallenge", connectionString);
    //    }
    }
}

