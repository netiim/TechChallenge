using Core.DTOs.ContatoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contratos.Request
{
    public class PostContatosRequest
    {
        public CreateContatoDTO CreateContatoDTO { get; set; }
    }
}
