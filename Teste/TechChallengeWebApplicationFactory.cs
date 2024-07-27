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
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Testes
{
    public class TechChallengeWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ApplicationDbContext Context { get; }
        private  string ContainerName = $"sql_server_test_container{DateTime.Now.Second.ToString()}";
        private const int HostPort = 1435;
        private const int ContainerPort = 1433;
        private string _databaseName;
        private string _connectionString;
        private DockerClient _dockerClient;
        private static readonly object Lock = new();
        private IContainer _sqlServerContainer;

        public TechChallengeWebApplicationFactory()
        {
            lock (Lock)
            {
                IServiceScope Scope = Services.CreateScope();
                _connectionString = GetConnectionString();
                Context = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); ;
                _dockerClient = new DockerClientConfiguration(new Uri(GetUri())).CreateClient();
                StartSqlServerContainer().Wait();
            }
        }
        private async Task StartSqlServerContainer()
        {
            await RemoveExistingContainerAsync();

            _sqlServerContainer = new ContainerBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:latest")
                .WithName(ContainerName)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SA_PASSWORD", "StrongPassword!123")
                .WithPortBinding(HostPort, ContainerPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(ContainerPort))
                .Build();

            Console.WriteLine("Iniciando o container do SQL Server...");
            await _sqlServerContainer.StartAsync();
            Console.WriteLine("Container iniciado. Aguardando disponibilidade do banco de dados...");

            await WaitUntilDatabaseIsAvailableAsync();
            await ApplyMigrationsAsync();
        }
        private async Task RemoveExistingContainerAsync()
        {
            using (var client = _dockerClient)
            {
                var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                foreach (var container in containers)
                {
                    if (container.Names.Contains("/" + ContainerName))
                    {
                        Console.WriteLine($"Removendo container existente: {ContainerName}");
                        await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true });
                    }
                }
            }
        }

        private async Task WaitUntilDatabaseIsAvailableAsync()
        {
            var maxRetries = 15;
            var delay = TimeSpan.FromSeconds(15);

            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    Console.WriteLine($"Tentativa {retry + 1} de conectar ao banco de dados...");
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        await connection.OpenAsync();
                        Console.WriteLine("Conexão estabelecida com sucesso.");
                        return; // Se conseguir abrir a conexão, saia do loop
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Tentativa {retry + 1} falhou: {ex.Message}");
                    await Task.Delay(delay);
                }
            }

            throw new Exception("Não foi possível conectar ao SQL Server após várias tentativas.");
        }

        private string GetConnectionString()
        {
            return $"Server=localhost,{HostPort};Database=master;User Id=sa;Password=StrongPassword!123;Encrypt=False;TrustServerCertificate=True;";
        }
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new ApplicationDbContext(options);
        }
        private static string GetUri()
        {
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            return isWindows ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
        }

        private static bool IsRunningInGitHubActions()
        {
            return Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
        }

        //private string GetConnectionString()
        //{
        //    return IsRunningInGitHubActions()
        //        ? "Server=localhost,1435;Database=TestTechChallenge;User Id=sa;Password=StrongPassword!123;Encrypt=False;TrustServerCertificate=True;"
        //        : $"Server=localhost,{HostPort};Database=TestTechChallenge;User Id=sa;Password=StrongPassword!123;Encrypt=False;TrustServerCertificate=True;";
        //}

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
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        })
                );
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

        private string GenerateDatabaseName()
        {
            return "TestTechChallenge_" + Guid.NewGuid().ToString().Substring(0, 5);
        }
        private async Task ApplyMigrationsAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            using var context = new ApplicationDbContext(options);
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}
