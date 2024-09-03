using Core.Entidades;

namespace Core.Interfaces.Repository;

public interface IEstadoRepository
{
    Task AdicionarEstadosEmMassa(List<Estado> estados);
    Task<List<Estado>> GetAll();
}
