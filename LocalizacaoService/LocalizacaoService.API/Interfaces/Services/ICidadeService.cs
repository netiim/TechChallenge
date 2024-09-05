using Core.Entidades;

namespace LocalizacaoService.Interfaces.Services;

public interface IRegiaoService
{
    Task<IEnumerable<Regiao>> ObterTodosAsync();
    Task CadastrarRegioesAsync();
}
