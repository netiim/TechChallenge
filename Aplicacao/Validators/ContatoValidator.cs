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
            RuleFor(contato => contato.Telefone)
           .Cascade(CascadeMode.Stop)
           .NotEmpty().WithMessage("O telefone não pode ser vazio.")
           .Must(t => t.ToString().Length == 11).WithMessage("O telefone deve ter 11 dígitos, incluindo o DDD.")
           .Must(t => t.ToString().All(char.IsDigit)).WithMessage("O telefone deve conter apenas números.")
           .MustAsync(ValidarDDD).WithMessage("O DDD não corresponde a uma região válida.");
        }

        private async Task<bool> ValidarDDD(int telefone, CancellationToken cancellationToken)
        {
            string ddd = telefone.ToString().Substring(0, 2);

            // Consultar o repositório para verificar se o DDD está presente
            var regiao = await _regiaoRepository.FindAsync(r => r.numeroDDD.ToString() == ddd);

            // Retorna verdadeiro se encontrar uma região com o DDD correspondente
            return regiao != null;
        }
    }
}
