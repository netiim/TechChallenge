using Core.Entidades;

namespace Core.Interfaces.Services;

public interface IEstadoService
{
    Task PreencherTabelaComEstadosBrasil();
    Task<IEnumerable<Estado>> ObterTodos();

}
