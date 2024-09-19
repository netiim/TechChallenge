using LocalizacaoService.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entidades
{
    public class Regiao : EntityBase,IEntity
    {
        /// <summary>
        /// Numero referente ao DDD da cidade
        /// </summary>
        [Range(minimum: 11, maximum: 99, MinimumIsExclusive = true, MaximumIsExclusive = true, ErrorMessage = "O número deve estar entre 11 e 99")]
        public int NumeroDDD { get; set; }
        [Required]
        public Estado Estado { get; set; } 
    }
}
