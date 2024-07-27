using Infraestrutura.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes
{
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
        }
    }
}
