using Core.Entidades;

namespace Core.Interfaces;

public interface IEstadoRepository
{
    void AdicionarEstadosEmMassa(List<Estado> estados);
}
