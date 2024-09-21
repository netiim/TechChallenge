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

        public async Task<Estado> AdicionarEstado(Estado estado)
        {
            await _context.Estado.AddAsync(estado);
            await _context.SaveChangesAsync();
            return estado;
        }

        public async Task<List<Estado>> GetAll()
        {
            List<Estado> estados = await _context.Estado.ToListAsync();
            return estados;
        }

        public async Task<Estado> BuscarEstadoPorSigla(string siglaEstado)
        {
            Estado estado = await _context.Estado.FirstOrDefaultAsync(x => x.siglaEstado.Equals(siglaEstado));
            return estado;
        }
    }
}
