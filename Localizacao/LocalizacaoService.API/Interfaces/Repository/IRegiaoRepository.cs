using Core.Entidades;
using LocalizacaoService._03_Repositorys;
using System.Linq.Expressions;

namespace LocalizacaoService.Interfaces.Repository;

public interface IRegiaoRepository : IBaseRepository<Regiao>
{
    Task<Regiao?> GetByDDDAsync(int ddd);
}
