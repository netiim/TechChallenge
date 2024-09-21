using Core.Entidades;
using System.Linq.Expressions;

namespace Core.Interfaces.Repository;

public interface IRegiaoRepository
{
    Task Adicionar(Regiao regiao);
    Task<IEnumerable<Regiao>> ObterTodosAsync();
    Task<IEnumerable<Regiao>> FindAsync(Expression<Func<Regiao, bool>> predicate);
}
