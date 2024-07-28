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
                await context.Database.OpenConnectionAsync();
                connected = true;
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection attempt {i + 1} failed: {ex.Message}");
                await Task.Delay(delay);
            }
        }

        if (!connected)
        {
            throw new Exception("Unable to connect to the database after multiple attempts.");
        }
    }
}