using Core.Entidades;

namespace Core.Interfaces;

public interface ICidadeRepository
{
    Task AdicionarCidadesEmMassa(List<Cidade> cidades);
}
