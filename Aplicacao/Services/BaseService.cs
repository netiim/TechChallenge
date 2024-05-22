using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services;

public class BaseService<T> : IBaseService<T> where T : EntityBase
{
    private readonly IBaseRepository<T> _repository;
    private readonly IValidator<T> _validator;

    public BaseService(IBaseRepository<T> repository, IValidator<T> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }
    public virtual async Task<T> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }
    public virtual async Task AdicionarAsync(T entity)
    {
        await ValidarPropriedades(entity);
        await _repository.AdicionarAsync(entity);
    }
    public virtual async Task AtualizarAsync(T entity)
    {
        await ValidarPropriedades(entity);
        await _repository.AtualizarAsync(entity);
    }
    public virtual async Task RemoverAsync(int id)
    {
        await _repository.RemoverAsync(id);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.FindAsync(predicate);
    }
    protected async Task ValidarPropriedades(T entity)
    {
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}