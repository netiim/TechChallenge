using Core.DTOs.UsuarioDTO;
using DotNet.Testcontainers.Builders;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Testcontainers.MsSql;
using static Core.Entidades.Usuario;

namespace Testes
{
    public class IntegrationTechChallengerWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
                        .UseSqlServer(_dbContainer.GetConnectionString());
                });


            });

            base.ConfigureWebHost(builder);
        }
        public async Task<HttpClient> GetClientWithAccessTokenAsync()
        {
            var client = CreateClient();

            // Criar o usuário e receber o token diretamente
            CreateUsuarioDTO newUser = new()
            {
                Username = "neto",
                Password = "123456",
                Perfil = PerfilUsuario.Administrador  
            };

            // Fazendo a requisição para criar o usuário e obter o token
            var response = await client.PostAsJsonAsync("/api/Token/criar-usuario", newUser);
            response.EnsureSuccessStatusCode();

            // Obter o token da resposta
            var token = await response.Content.ReadAsStringAsync();

            // Adicionar o token no cabeçalho de autorização para as requisições seguintes
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
