using Core.Entidades;
namespace LocalizacaoService.Interfaces.Services;
public interface IEstadoService
{
    Task<List<Estado>> BuscarEstadosBrasilAsync();
}
