﻿using Core.DTOs.ContatoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ContatoDTO
{
    public class ContatosResponse
    {
        public List<ReadContatoDTO> Contatos { get; set; }
    }
}