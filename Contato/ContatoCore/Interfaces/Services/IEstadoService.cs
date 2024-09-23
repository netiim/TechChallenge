using Core.Entidades;

namespace Core.Interfaces.Services;

public interface IEstadoService
{
    Task<IEnumerable<Estado>> ObterTodos();

}
