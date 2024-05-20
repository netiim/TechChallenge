using Core.Entidades;
using Core.Interfaces.Repository;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly ApplicationDbContext _context;

        public RegiaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Regiao>> ObterTodosAsync()
        {
            return await _context.Regiao.ToListAsync();
        }
        public async Task Adicionar(Regiao regiao)
        {
             await _context.Regiao.AddAsync(regiao);
             await _context.SaveChangesAsync();
        }
    }
}
