using Core.DTOs.UsuarioDTO;
using Docker.DotNet;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Testcontainers.MsSql;

namespace Testes
{
    public class TechChallengeWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ApplicationDbContext Context { get; }
        public IServiceScope scope;
        private const string ContainerName = "sql_server_test_container";
        private const int HostPort = 1435;
        private const int ContainerPort = 1433;
        private string _connectionString;
        private DockerClient _dockerClient;
        private static readonly object Lock = new();
        private IContainer _sqlServerContainer;

        public TechChallengeWebApplicationFactory()
        {
            this.scope = Services.CreateScope();
            _connectionString = $"Server=localhost,1435;Database=TechChallenge;User Id=sa;Password=StrongPassword!123;Encrypt=False;TrustServerCertificate=True;";
            Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();  
            _dockerClient = new DockerClientConfiguration(new Uri(GetUri())).CreateClient();       
        }

        private static string GetUri()
        {
            var isWindows = System.Runtime.InteropServices.RuntimeInformation
                                    .IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            var dockerUri = isWindows ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";

            return dockerUri;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(_connectionString, sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        })
                );
            });

            base.ConfigureWebHost(builder);
        }
        public async Task<HttpClient> GetClientWithAccessTokenAsync()
        {
            var client = this.CreateClient();

            var user = new UsuarioTokenDTO { Username = "netim", Password = "123456" };

            var resultado = await client.PostAsJsonAsync("/api/Token", user);

            resultado.EnsureSuccessStatusCode();

            var result = await resultado.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);

            return client;
        }

        public async Task InitializeAsync()
        {
            lock (Lock)
            {
                try
                {
                    var containers = _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true }).GetAwaiter().GetResult();
                    var existingContainer = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName));

                    if (existingContainer == null)
                    {
                        _sqlServerContainer = new ContainerBuilder()
                            .WithImage("mcr.microsoft.com/mssql/server:latest")
                            .WithName(ContainerName)
                            .WithEnvironment("ACCEPT_EULA", "Y")
                            .WithEnvironment("SA_PASSWORD", "StrongPassword!123")
                            .WithPortBinding(HostPort, ContainerPort)
                            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(ContainerPort))
                            .Build();

                        _sqlServerContainer.StartAsync().GetAwaiter().GetResult();
                    }
                    else
                    {
                        _dockerClient.Containers.StartContainerAsync(existingContainer.ID, new ContainerStartParameters()).GetAwaiter().GetResult();
                    }
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to initialize Docker container", ex);
                }
            }

            _connectionString = $"Server=localhost,{HostPort};Database=TechChallenge;User Id=sa;Password=StrongPassword!123;Encrypt=False;TrustServerCertificate=True;";
            
            await ApplyMigrationsAsync();
        }

        public async Task DisposeAsync()
        {
            lock (Lock)
            {
                if (_sqlServerContainer != null)
                {
                    _sqlServerContainer.StopAsync().GetAwaiter().GetResult();
                    _sqlServerContainer.DisposeAsync().GetAwaiter().GetResult();
                }
                else
                {
                    var containers = _dockerClient?.Containers.ListContainersAsync(new ContainersListParameters { All = true }).GetAwaiter().GetResult();
                    var existingContainer = containers?.FirstOrDefault(c => c.Names.Contains("/" + ContainerName));

                    if (existingContainer != null)
                    {
                        _dockerClient?.Containers.StopContainerAsync(existingContainer.ID, new ContainerStopParameters()).GetAwaiter().GetResult();
                    }
                }
            }
        }

        private async Task ApplyMigrationsAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.MigrateAsync();
        }
    }
}
