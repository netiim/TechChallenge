
using Core.Entidades;
using System.Linq.Expressions;

namespace Core.Interfaces.Services;

public interface IContatoService : IBaseService<Contato>
{    
    Task AdicionarAsync(Contato entity);    
    Task AtualizarAsync(Contato entity);    
}
