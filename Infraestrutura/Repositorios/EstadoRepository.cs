using Core.Entidades;
using Core.Interfaces;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AdicionarEstadosEmMassa(List<Estado> estados)
        {
            //foreach (Estado estado in estados)
            //{
            
            _context.Estado.AddRange(estados);
                _context.SaveChanges();
            //}    
        }
    }
}
