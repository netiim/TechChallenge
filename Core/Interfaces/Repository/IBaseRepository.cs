using Core.Entidades;
using System.Linq.Expressions;

namespace Core.Interfaces.Repository;

public interface IBaseRepository<T> where T : EntityBase
{
    Task<IEnumerable<T>> ObterTodosAsync();
    Task<T> ObterPorIdAsync(int id);
    Task AdicionarAsync(T entity);
    Task AtualizarAsync(T entity);
    Task RemoverAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
