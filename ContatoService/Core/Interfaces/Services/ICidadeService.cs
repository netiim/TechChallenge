using Core.Entidades;

namespace Core.Interfaces.Services;

public interface IRegiaoService
{
    Task<IEnumerable<Regiao>> ObterTodosAsync();
    Task PreencherRegioesComDDD();
}
