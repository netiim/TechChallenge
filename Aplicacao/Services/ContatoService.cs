using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation;
using System.Linq.Expressions;
using System.Threading;

namespace Aplicacao.Services;

public class ContatoService : IContatoService
{
    protected readonly IContatoRepository _repository;
    private readonly IValidator<Contato> _validator;
    private readonly IRegiaoRepository _regiaoRepository;

    public ContatoService(IContatoRepository repository, IValidator<Contato> validator, IRegiaoRepository regiaoRepository)
    {
        _repository = repository;
        _validator = validator;
        _regiaoRepository = regiaoRepository;
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
        ValidacaoTelefone(entity);

        string ddd = entity.Telefone.ToString().Substring(0, 2);
        var list = await _regiaoRepository.FindAsync(r => r.numeroDDD.ToString() == ddd);
        entity.RegiaoId = list.FirstOrDefault().Id;

        await _repository.AdicionarAsync(entity);
    }

    private void ValidacaoTelefone(Contato entity)
    {
        var result = _validator.Validate(entity);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
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
