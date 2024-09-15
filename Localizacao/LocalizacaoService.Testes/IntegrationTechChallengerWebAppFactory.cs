using LocalizacaoService._03_Repositorys.Config;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Testes
{
    public class IntegrationTechChallengerWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(27017, true)
        .Build();
        public async Task InitializeAsync()
        {
            await _mongoDbContainer.StartAsync();
            Environment.SetEnvironmentVariable("MONGO_DB_CONNECTION_STRING", _mongoDbContainer.GetConnectionString());
        }
        public new async Task DisposeAsync()
        {
            await _mongoDbContainer.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<MongoDbSettings>(options =>
                {
                    options.ConnectionString = _mongoDbContainer.GetConnectionString();
                    options.DatabaseName = "TestDatabase";
                });

                services.AddSingleton<IMongoClient, MongoClient>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    return new MongoClient(settings.ConnectionString);
                });

                services.AddScoped(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    var client = sp.GetRequiredService<IMongoClient>();
                    return client.GetDatabase(settings.DatabaseName);
                });

                var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IBus));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddMassTransit(x =>
                {
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
                });

                services.RemoveAll(typeof(IHostedService));
            });
            
            base.ConfigureWebHost(builder);
        }
    }
}
