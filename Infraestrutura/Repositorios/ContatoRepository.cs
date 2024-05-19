using Core.Entidades;
using Infraestrutura.Data;

namespace Infraestrutura.Repositorios;

public class ContatoRepository : BaseRepository<Contato>
{
    public ContatoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
