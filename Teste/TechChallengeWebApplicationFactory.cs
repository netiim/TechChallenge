using Core.DTOs.UsuarioDTO;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Testes
{
    public class TechChallengeWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ApplicationDbContext Context { get; }
        public IServiceScope scope;
        public TechChallengeWebApplicationFactory()
        {
            this.scope = Services.CreateScope();
            Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer("Server=localhost;Database=TechChallenge;User ID=sa;Password=YourStrongPassword1!;TrustServerCertificate=True;")
                );
            });

            base.ConfigureWebHost(builder);
        }
        public async Task<HttpClient> GetClientWithAccessTokenAsync()
        {
            var client = this.CreateClient();

            var user = new UsuarioTokenDTO { Username = "netim", Password = "123456" };

            var resultado = await client.PostAsJsonAsync("/auth-login", user);

            resultado.EnsureSuccessStatusCode();

            var result = await resultado.Content.ReadFromJsonAsync<string>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);

            return client;
        }

    }
}
