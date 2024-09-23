using AutoMapper;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.Contratos.Response;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit;

namespace ContatoWorker.Delete.Consumers
{
    public class DeleteContatoConsumer : IConsumer<DeleteContatoRequest>
    {
        private readonly IContatoService _contatoService;

        public DeleteContatoConsumer(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        public async Task Consume(ConsumeContext<DeleteContatoRequest> context)
        {
            try
            {
                var contato = await BuscarContatoExistenteBanco(context);

                if (contato == null) return;

                await RemoverContato(context, contato);
            }
            catch (Exception e)
            {
                await context.RespondAsync<ContatoErroResponse>(new ContatoErroResponse { MensagemErro = $"{e.Message}" });
                throw;
            }            
        }

        private async Task RemoverContato(ConsumeContext<DeleteContatoRequest> context, Contato? contato)
        {
            await _contatoService.RemoverAsync(contato.Id);
            await context.RespondAsync<ContatoSucessResponse>(new ContatoSucessResponse());
        }

        private async Task<Contato?> BuscarContatoExistenteBanco(ConsumeContext<DeleteContatoRequest> context)
        {
            var contato = await _contatoService.ObterPorIdAsync(context.Message.Id);
            if (contato == null)
            {
                await context.RespondAsync<ContatoNotFound>(new ContatoNotFound { Mensagem = "Contato não foi encontrado no banco de dados" });
            }
            return contato;
        }
    }
}
