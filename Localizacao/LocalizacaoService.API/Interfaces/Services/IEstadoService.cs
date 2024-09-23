using Core.Entidades;
using MappingRabbitMq.Models;
namespace LocalizacaoService.Interfaces.Services;
public interface IEstadoService
{
    Task<List<ReadEstadoDTO>> BuscarEstadosBrasilAsync();
}
