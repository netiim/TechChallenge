using Core.Entidades;

namespace Core.Interfaces;

public interface IEstadoRepository
{
    Task AdicionarEstadosEmMassa(List<Estado> estados);
    Task<List<Estado>> GetAll();
}
