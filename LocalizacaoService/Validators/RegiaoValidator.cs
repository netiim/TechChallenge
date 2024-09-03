using Core.DTOs.RegiaoDTO;
using Core.Entidades;
using LocalizacaoService.Interfaces.Validators;

namespace LocalizacaoService.Validators;

public class RegiaoValidator : IRegiaoValidator
{
    public void Validar(RegiaoAPIDTO regiaoApiDTO, List<Estado> estados)
    {
        ValidaNullidadeObjetoAPI(regiaoApiDTO);
        ValidaSiglaExisteNaTabelaEstado(regiaoApiDTO, estados);
    }

    private void ValidaSiglaExisteNaTabelaEstado(RegiaoAPIDTO regiaoApiDTO, List<Estado> estados)
    {
        if (estados.FirstOrDefault(x => x.siglaEstado.Equals(regiaoApiDTO.state)) == null)
        {
            throw new Exception("Nenhuma sigla correspondente a sigla retornada pela API");
        }
    }

    private static void ValidaNullidadeObjetoAPI(RegiaoAPIDTO regiaoApiDTO)
    {
        if (regiaoApiDTO == null)
        {
            throw new Exception("Objeto API vazio");
        }
    }
}
