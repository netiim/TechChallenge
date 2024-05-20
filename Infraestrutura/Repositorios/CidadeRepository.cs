using Core.Entidades;
using Core.Interfaces.Repository;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios
{
    public class CidadeRepository : ICidadeRepository
    {
        private readonly ApplicationDbContext _context;

        public CidadeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cidade>> ObterTodosAsync()
        {
            return await _context.Cidade.ToListAsync();
        }
        public async Task AdicionarCidadesEmMassa(List<Cidade> cidades)
        {
             await _context.Cidade.AddRangeAsync(cidades);
             await _context.SaveChangesAsync();
        }
    }
}
