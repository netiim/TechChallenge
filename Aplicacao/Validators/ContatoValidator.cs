using Core.Entidades;
using Core.Interfaces.Repository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Validators
{
    public class ContatoValidator : AbstractValidator<Contato>
    {
        private readonly IRegiaoRepository _regiaoRepository;
        public ContatoValidator(IRegiaoRepository regiaoRepository)
        {
            _regiaoRepository = regiaoRepository;

            RuleFor(contato => contato.Nome)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("O nome deve conter apenas caracteres alfabéticos.");

            RuleFor(contato => contato.Email)
                .NotEmpty().WithMessage("O email não pode ser vazio.")
                .EmailAddress().WithMessage("O email deve ser válido.");

            RuleFor(contato => contato.Telefone)
           .Cascade(CascadeMode.Stop)
           .NotEmpty().WithMessage("O telefone não pode ser vazio.")
           .Must(t => t.ToString().Length == 11).WithMessage("O telefone deve ter 11 dígitos, incluindo o DDD.")
           .Must(t => t.ToString().All(char.IsDigit)).WithMessage("O telefone deve conter apenas números.")
           .MustAsync(ValidarDDD).WithMessage("O DDD não corresponde a uma região válida.");
        }

        private async Task<bool> ValidarDDD(string telefone, CancellationToken token)
        {
            string ddd = telefone.Substring(0, 2);

            var regiao = await _regiaoRepository.FindAsync(r => r.numeroDDD.ToString() == ddd);

            return regiao != null && regiao.Count() > 0;
        }
    }
}
