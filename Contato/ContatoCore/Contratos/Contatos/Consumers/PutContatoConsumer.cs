using AutoMapper;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit;

namespace ContatoWorker.Put.Consumers
{
    public class PutContatoConsumer : IConsumer<PutContatoRequest>
    {
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        public PutContatoConsumer(IContatoService contatoService, IMapper mapper)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PutContatoRequest> context)
        {
            try
            {
                var contato = await _contatoService.ObterPorIdAsync(context.Message.ContatoDTO.Id);   

                if (contato == null)
                {
                    await context.RespondAsync<ContatoNotFound>(new ContatoNotFound { Mensagem = "Contato não foi encontrado no banco de dados" });
                    return;
                }
                _mapper.Map(context.Message.ContatoDTO, contato);

                // Atualiza o contato no banco de dados
                await AtualizarContato(context, contato);
            }
            catch (Exception e)
            {
                await context.RespondAsync<ContatoErroResponse>(new ContatoErroResponse { MensagemErro = $"{e.Message}" });
                throw;
            }
        }
        private async Task AtualizarContato(ConsumeContext<PutContatoRequest> context, Contato contato)
        {
            await _contatoService.AtualizarAsync(contato);
            await context.RespondAsync<ContatoResponse>(new ContatoResponse { Contato = _mapper.Map<ReadContatoDTO>(contato) });
        }
    }
}
