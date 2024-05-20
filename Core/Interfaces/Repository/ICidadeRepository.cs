using Core.Entidades;

namespace Core.Interfaces.Repository;

public interface IRegiaoRepository
{
    Task Adicionar(Regiao regiao);
    Task<IEnumerable<Regiao>> ObterTodosAsync();
}
