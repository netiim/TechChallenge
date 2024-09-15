using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Interfaces.Services;
using MassTransit;

namespace ContatoWorker.Delete.Consumers
{
    public class DeleteContatoConsumer : IConsumer<DeleteContatoDTO>
    {
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        public DeleteContatoConsumer(IContatoService contatoService, IMapper mapper)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<DeleteContatoDTO> context)
        {
            var contato = context.Message;

            await _contatoService.RemoverAsync(contato.Id);

            await context.ConsumeCompleted;
        }
    }
}
