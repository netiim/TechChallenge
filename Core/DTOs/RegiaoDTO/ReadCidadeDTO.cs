using Core.DTOs.EstadoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.RegiaoDTO
{
    public class ReadRegiaoDTO
    {
        public int numeroDDD { get; set; }
        public int EstadoId { get; set; }
        public ReadEstadoDTO Estado { get; set; }
    }
}
