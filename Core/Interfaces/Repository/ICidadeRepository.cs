using Core.Entidades;

namespace Core.Interfaces.Repository;

public interface ICidadeRepository
{
    Task AdicionarCidadesEmMassa(List<Cidade> cidades);
    Task<IEnumerable<Cidade>> ObterTodosAsync();
}
