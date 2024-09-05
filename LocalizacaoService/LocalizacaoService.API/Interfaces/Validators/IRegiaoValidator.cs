using Core.DTOs.RegiaoDTO;
using Core.Entidades;

namespace LocalizacaoService.Interfaces.Validators;

public interface IRegiaoValidator
{
    void Validar(RegiaoAPIDTO regiaoApiDTO, List<Estado> estados);
}
