using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if(context.Message.ContatoId > 0)
                {
                    contatos.Add(await _contatoRepository.ObterPorIdAsync(context.Message.ContatoId));
                }
                else if (context.Message.NumeroDDD > 0)
                {
                    contatos = (await _contatoRepository.FindAsync(c => c.Regiao.NumeroDDD == context.Message.NumeroDDD)).ToList();
                }
                else
                {
                    contatos = (await _contatoRepository.ObterTodosAsync()).ToList();
                }
                _logger.LogInformation($"Obteve {contatos.Count()} contatos do repositório.");

                ContatosResponse response = new ContatosResponse
                {
                    Contatos = _mapper.Map<List<ReadContatoDTO>>(contatos)
                };

                await context.RespondAsync<ContatosResponse>(response);

                _logger.LogInformation("Resposta enviada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a requisição de obter contatos.");
                throw;
            }
        }
    }
}

