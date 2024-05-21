using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
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

    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
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
        await _repository.AdicionarAsync(entity);
    }
    public virtual async Task AtualizarAsync(T entity)
    {
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
}