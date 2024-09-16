using AutoMapper;
using Azure;
using Core.Contratos.Contatos;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ContatoWorker.Get.Consumers
{
    public class GetContatosConsumer : IConsumer<GetContatosRequest>
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger<GetContatosConsumer> _logger;
        private readonly IMapper _mapper;

        public GetContatosConsumer(IContatoRepository contatoRepository, ILogger<GetContatosConsumer> logger, IMapper mapper)
        {
            _contatoRepository = contatoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetContatosRequest> context)
        {
            try
            {
                _logger.LogInformation("Recebida requisição para obter contatos.");

                List<Contato> contatos = new List<Contato>();

                if (context.Message.ContatoId > 0)
                {
                    await BuscarContatoPorId(context, contatos);
                }
                else if (context.Message.NumeroDDD > 0)
                {
                    await BuscarContatoPorDDD(context, contatos);
                }
                else
                {
                    contatos = (await _contatoRepository.ObterTodosAsync()).ToList();
                    await EnviarMensagemContatoResponse(context, contatos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a requisição de obter contatos.");
                await context.RespondAsync<ContatoErroResponse>(new ContatoErroResponse { MensagemErro = $"{ex.Message}" });
                throw;
            }
        }

        private async Task BuscarContatoPorDDD(ConsumeContext<GetContatosRequest> context, List<Contato> contatos)
        {
            contatos = (await _contatoRepository.FindAsync(c => c.Regiao.NumeroDDD == context.Message.NumeroDDD)).ToList();
            await RetornarMensagem(context, contatos);
        }

        private async Task RetornarMensagem(ConsumeContext<GetContatosRequest> context, List<Contato> contatos)
        {

            if (contatos == null || contatos.Count == 0)
            {
                await context.RespondAsync<ContatoNotFound>(new ContatoNotFound { Mensagem = "Contato não foi encontrado no banco de dados" });
            }
            else
            {
                await EnviarMensagemContatoResponse(context, contatos);
            }
        }

        private async Task EnviarMensagemContatoResponse(ConsumeContext<GetContatosRequest> context, List<Contato> contatos)
        {

            ContatosResponse response = new ContatosResponse
            {
                Contatos = _mapper.Map<List<ReadContatoDTO>>(contatos)
            };
            await context.RespondAsync<ContatosResponse>(response);
        }

        private async Task BuscarContatoPorId(ConsumeContext<GetContatosRequest> context, List<Contato> contatos)
        {
            Contato contato = await _contatoRepository.ObterPorIdAsync(context.Message.ContatoId);
            if (contato != null)
                contatos.Add(contato);

           await RetornarMensagem(context, contatos);
        }
    }
}

