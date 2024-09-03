using Core.DTOs.UsuarioDTO;
using DotNet.Testcontainers.Builders;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

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
            var user = new UsuarioTokenDTO { Username = "netim", Password = "123456" };
            var resultado = await client.PostAsJsonAsync("/api/Token", user);
            resultado.EnsureSuccessStatusCode();
            var result = await resultado.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);
            return client;
        }
        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
