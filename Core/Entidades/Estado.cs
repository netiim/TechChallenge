using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Estado : EntityBase
    {
        public string Nome { get; set; }

        /// <summary>
        /// Sigla do estado referente a cidade
        /// </summary>
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla do estado deve ter exatamente 2 caracteres.")]
        public string siglaEstado { get; set; }
    }
}
