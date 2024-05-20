using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ContatoDTO
{
    public class CreateContatoDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }
        public int CidadeId { get; set; }
    }
}
