using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testes;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTechChallengerWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IntegrationTechChallengerWebAppFactory app;
    protected readonly ApplicationDbContext _context;

    protected BaseIntegrationTest(IntegrationTechChallengerWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        app = factory;
        _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        WaitForDatabase(_context).Wait();
        _context.Database.MigrateAsync().Wait();
    }

    private async Task WaitForDatabase(ApplicationDbContext context)
    {
        var retries = 10;
        var delay = TimeSpan.FromSeconds(5);
        var connected = false;

        for (int i = 0; i < retries; i++)
        {
            try
            {
                Console.WriteLine($"Tentando conectar ao banco de dados, tentativa {i + 1} de {retries}...");
                await context.Database.OpenConnectionAsync();
                connected = true;
                Console.WriteLine("Conexão com o banco de dados bem-sucedida.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tentativa de conexão com o banco de dados {i + 1} falhou: {ex.Message}");
                await Task.Delay(delay);
            }
        }

        if (!connected)
        {
            throw new Exception("Não foi possível conectar ao banco de dados após várias tentativas.");
        }
    }
}