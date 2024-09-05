using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.RegiaoDTO
{
    public class RegiaoAPIDTO
    {
        public string state { get; set; }
        public List<string> cities { get; set; }
    }
}
