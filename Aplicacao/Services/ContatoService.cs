using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using System.Linq.Expressions;

namespace Aplicacao.Services;

public class ContatoService : IContatoService
{
    protected readonly IContatoRepository _repository;

    public ContatoService(IContatoRepository repository)
    {
        _repository = repository;
    }

    public virtual async Task<IEnumerable<Contato>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public virtual async Task<Contato> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public virtual async Task AdicionarAsync(Contato entity)
    {
        await _repository.AdicionarAsync(entity);
    }

    public virtual async Task AtualizarAsync(Contato entity)
    {
        await _repository.AtualizarAsync(entity);
    }

    public virtual async Task RemoverAsync(int id)
    {
        await _repository.RemoverAsync(id);
    }

    public virtual async Task<IEnumerable<Contato>> FindAsync(Expression<Func<Contato, bool>> predicate)
    {
        return await _repository.FindAsync(predicate);
    }
}
