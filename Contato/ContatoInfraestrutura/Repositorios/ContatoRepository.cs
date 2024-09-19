using Core.Entidades;
using Core.Interfaces.Repository;
using Infraestrutura.Data;

namespace Infraestrutura.Repositorios;

public class ContatoRepository : BaseRepository<Contato>,IContatoRepository
{
    public ContatoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
