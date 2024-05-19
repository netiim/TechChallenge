using Core.Interfaces.Repository;

namespace Aplicacao.Services;

public class ContatoService
{
    private readonly IContatoRepository _repository;

    public ContatoService(IContatoRepository repository)
    {
        _repository = repository;
    }
}
