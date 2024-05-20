using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Cidade : EntityBase
    {
        /// <summary>
        /// Nome da cidade 
        /// </summary>        
        public string Nome { get; set; }
        /// <summary>
        /// Numero referente ao DDD da cidade
        /// </summary>
        [Range(minimum: 11, maximum: 99, MinimumIsExclusive = true, MaximumIsExclusive = true, ErrorMessage = "O número deve estar entre 11 e 99")]
        public int numeroDDD { get; set; }
        /// <summary>
        /// Id do estado ao qual aquela cidade pertence
        /// </summary>
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; }

        public virtual ICollection<Contato> Contatos { get; set; }
    }
}
