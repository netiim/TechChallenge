using System.ComponentModel.DataAnnotations;

namespace Core.Entidades
{
    public class Regiao : EntityBase
    {
        /// <summary>
        /// Numero referente ao DDD da cidade
        /// </summary>
        [Range(minimum: 11, maximum: 99, MinimumIsExclusive = true, MaximumIsExclusive = true, ErrorMessage = "O número deve estar entre 11 e 99")]
        public int NumeroDDD { get; set; }
        /// <summary>
        /// Id do estado ao qual aquela cidade pertence
        /// </summary>
        public int EstadoId { get; set; }

        public string IdLocalidadeAPI { get; set; }

        public virtual Estado Estado { get; set; }

        public virtual ICollection<Contato> Contatos { get; set; }
    }
}
