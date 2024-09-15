using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatoWorker.Put.Consumers
{
    public class PutContatoConsumer : IConsumer<PutContatoDTO>
    {
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        public PutContatoConsumer(IContatoService contatoService, IMapper mapper)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PutContatoDTO> context)
        {
            var contatoDTO = context.Message;
            Contato contato = _mapper.Map<Contato>(contatoDTO);

            await _contatoService.AtualizarAsync(contato);

            await context.ConsumeCompleted;
        }
    }
}
