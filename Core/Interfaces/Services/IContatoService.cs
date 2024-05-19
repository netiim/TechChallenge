
using Core.Entidades;
using System.Linq.Expressions;

namespace Core.Interfaces.Services;

public interface IContatoService 
{
    Task<IEnumerable<Contato>> ObterTodosAsync();
    Task<Contato> ObterPorIdAsync(int id);
    Task AdicionarAsync(Contato entity);
    Task AtualizarAsync(Contato entity);
    Task RemoverAsync(int id);
    Task<IEnumerable<Contato>> FindAsync(Expression<Func<Contato, bool>> predicate);
}
