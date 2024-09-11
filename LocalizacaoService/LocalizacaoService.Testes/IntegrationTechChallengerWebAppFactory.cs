using LocalizacaoService._03_Repositorys.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net.Http.Headers;
using Testcontainers.MongoDb;

namespace Testes
{
    public class IntegrationTechChallengerWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(27017, true) // Porta padrão do MongoDB
        .Build();
        public async Task InitializeAsync()
        {
            // Startar o container do MongoDB
            await _mongoDbContainer.StartAsync();

            // Configurar a string de conexão do MongoDB nos testes
            Environment.SetEnvironmentVariable("MONGO_DB_CONNECTION_STRING", _mongoDbContainer.GetConnectionString());
        }

        // Limpeza após os testes
        public new async Task DisposeAsync()
        {
            // Parar o container após a execução dos testes
            await _mongoDbContainer.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configurar as opções do MongoDB para testes
                services.Configure<MongoDbSettings>(options =>
                {
                    options.ConnectionString = _mongoDbContainer.GetConnectionString();
                });

                // Injetar o MongoClient como Singleton
                services.AddSingleton<IMongoClient, MongoClient>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    return new MongoClient(settings.ConnectionString);
                });

                // Injetar o IMongoDatabase como Scoped
                services.AddScoped(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    var client = sp.GetRequiredService<IMongoClient>();
                    return client.GetDatabase(settings.DatabaseName);
                });

                // Registrar outras dependências conforme necessário
            });

            base.ConfigureWebHost(builder);
        }
        //public async Task<HttpClient> GetClientWithAccessTokenAsync()
        //{
        //    var client = CreateClient();

        //    // Criar o usuário e receber o token diretamente
        //    CreateUsuarioDTO newUser = new()
        //    {
        //        Username = "neto",
        //        Password = "123456",
        //        Perfil = PerfilUsuario.Administrador  
        //    };                                                                                                   

        //    // Fazendo a requisição para criar o usuário e obter o token
        //    var response = await client.PostAsJsonAsync("/api/Token/criar-usuario", newUser);
        //    response.EnsureSuccessStatusCode();

        //    // Obter o token da resposta
        //    var token = await response.Content.ReadAsStringAsync();

        //    // Adicionar o token no cabeçalho de autorização para as requisições seguintes
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    return client;
        //}
    }
}
