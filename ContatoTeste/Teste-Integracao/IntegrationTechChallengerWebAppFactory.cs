using Core.DTOs.UsuarioDTO;
using Core.Entidades;
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
        public async Task<HttpClient> GetClientWithAccessTokenAsync(Usuario usuario)
        {
            var client = CreateClient();

            UsuarioTokenDTO usuarioTokenDTO = new()
            {
                Username = usuario.Username,
                Password = usuario.Password
            };

            var response = await client.PostAsJsonAsync("/api/Token/CriarToken", usuario);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
