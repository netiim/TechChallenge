using Contatos.Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace Contatos.Aplicacao.Services;

public class DatabaseService : IDatabaseService
{
    private readonly ApplicationDbContext _context;

    public DatabaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void MigrateDatabase()
    {
        _context.Database.Migrate();
    }
}
