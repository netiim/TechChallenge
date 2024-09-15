using Core.Entidades;

namespace Core.Interfaces.Repository;

public interface IEstadoRepository
{
    Task<Estado> AdicionarEstado(Estado estados);
    Task<List<Estado>> GetAll();
    Task<Estado> BuscarEstadoPorSigla(string siglaEstado);
}
