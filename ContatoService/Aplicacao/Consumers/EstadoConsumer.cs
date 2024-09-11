using Castle.Core.Logging;
using Core.Entidades;
using Core.Interfaces.Repository;
using MappingRabbitMq.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Aplicacao.Consumers
{
    public class EstadoConsumer : IConsumer<ReadEstadoDTO>
    {
        private readonly ILogger<RegiaoConsumer> _logger;
        private readonly IEstadoRepository _estadoRepository;
        public EstadoConsumer(ILogger<RegiaoConsumer> logger, IRegiaoRepository regiaoRepository, IEstadoRepository estadoRepository)
        {
            _logger = logger;
            _estadoRepository = estadoRepository;
        }

        public async Task Consume(ConsumeContext<ReadEstadoDTO> context)
        {
            try
            {
                if (await _estadoRepository.BuscarEstadoPorSigla(context.Message.siglaEstado) != null)
                {
                    await Task.CompletedTask;
                }

                Estado estado = new Estado()
                {
                    Nome = context.Message.Nome,
                    siglaEstado = context.Message.siglaEstado
                };

                await _estadoRepository.AdicionarEstado(estado);

                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar a mensagem");
                throw;
            }
        }
    }
}
