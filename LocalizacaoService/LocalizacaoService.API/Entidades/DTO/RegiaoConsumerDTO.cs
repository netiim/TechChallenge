using System.ComponentModel.DataAnnotations;

namespace MappingRabbitMq.Models
{
    public class RegiaoConsumerDTO
    {
        public string Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        /// <summary>
        /// Numero referente ao DDD da cidade
        /// </summary>
        [Range(minimum: 11, maximum: 99, MinimumIsExclusive = true, MaximumIsExclusive = true, ErrorMessage = "O número deve estar entre 11 e 99")]
        public int NumeroDDD { get; set; }

        public string siglaEstado { get; set; }
    }
}


