using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using FluentValidation;
using System.Linq.Expressions;
using System.Threading;

namespace Aplicacao.Services;

public class ContatoService : BaseService<Contato>,IContatoService
{
    protected readonly IContatoRepository _contatoRepository;
    private readonly IValidator<Contato> _validator;
    private readonly IRegiaoRepository _regiaoRepository;

    public ContatoService(IContatoRepository contatorepository, IValidator<Contato> validator, IRegiaoRepository regiaoRepository)
        :base(contatorepository, validator)
    {
        _contatoRepository = contatorepository;
        _validator = validator;
        _regiaoRepository = regiaoRepository;
    }  

    public override async Task AdicionarAsync(Contato entity)
    {
        await ValidarPropriedades(entity);

        string ddd = entity.Telefone.ToString().Substring(0, 2);
        var list = await _regiaoRepository.FindAsync(r => r.NumeroDDD.ToString() == ddd);
        entity.RegiaoId = list.FirstOrDefault().Id;

        await _contatoRepository.AdicionarAsync(entity);
    }

    private async Task ValidarPropriedades(Contato entity)
    {
        var result = await _validator.ValidateAsync(entity);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }   
}
