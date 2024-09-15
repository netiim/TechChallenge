using Core.DTOs.RegiaoDTO;
using MappingRabbitMq.Models;

namespace LocalizacaoService.Interfaces.Validators;

public interface IRegiaoValidator
{
    void Validar(RegiaoAPIDTO? regiaoApiDTO, List<ReadEstadoDTO>? estados);
}
