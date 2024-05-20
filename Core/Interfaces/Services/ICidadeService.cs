using Core.Entidades;

namespace Core.Interfaces.Services;

public interface ICidadeService
{
    Task<IEnumerable<Cidade>> ObterTodosAsync();
    Task PreencherCidadesComDDD();
}
