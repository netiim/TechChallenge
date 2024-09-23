using Core.Entidades;
using Core.Interfaces.Repository;
using Infraestrutura.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Adicionar(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Usuario>> ListarUsuarios()
        {
            return _context.Usuario.ToList();
        }
    }
}
