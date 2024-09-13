using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit;

namespace ContatoWorker.Post.Consumers
{
    public class PostContatoConsumer : IConsumer<CreateContatoDTO>
    {
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        public PostContatoConsumer(IContatoService contatoService, IMapper mapper)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CreateContatoDTO> context)
        {
            var contatoDTO = context.Message;
            Contato contato = _mapper.Map<Contato>(contatoDTO); 

            await _contatoService.AdicionarAsync(contato);

            await context.ConsumeCompleted;
        }
    }
}
