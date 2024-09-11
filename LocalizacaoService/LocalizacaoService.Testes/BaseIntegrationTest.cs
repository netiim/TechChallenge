using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Testes;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTechChallengerWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IntegrationTechChallengerWebAppFactory app;
    protected readonly IMongoDatabase _database;

    protected BaseIntegrationTest(IntegrationTechChallengerWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        app = factory;

        var client = _scope.ServiceProvider.GetRequiredService<IMongoClient>();
        _database = client.GetDatabase("TestDatabase");  
        WaitForDatabase(_database).Wait();
    }

    private async Task WaitForDatabase(IMongoDatabase database)
    {
        var retries = 10;
        var delay = TimeSpan.FromSeconds(5);
        var connected = false;

        for (int i = 0; i < retries; i++)
        {
            try
            {
                var command = new BsonDocument("ping", 1);
                await database.RunCommandAsync<BsonDocument>(command);

                Console.WriteLine("Database connection successful.");
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