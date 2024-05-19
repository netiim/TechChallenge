using Core.Entidades;
using Core.Interfaces.Repository;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infraestrutura.Repositorios
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarEstadosEmMassa(List<Estado> estados)
        {
            await _context.Estado.AddRangeAsync(estados);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Estado>> GetAll()
        {
            List<Estado> estados = await _context.Estado.ToListAsync();
            return estados;
        }
    }
}
