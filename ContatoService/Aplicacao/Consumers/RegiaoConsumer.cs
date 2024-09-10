using Core.Entidades;
using Core.Interfaces.Repository;
using MappingRabbitMq.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Aplicacao.Consumers
{
    public class RegiaoConsumer : IConsumer<RegiaoConsumerDTO>
    {
        private readonly ILogger<RegiaoConsumer> _logger;
        private readonly IRegiaoRepository _regiaoRepository;
        private readonly IEstadoRepository _estadoRepository;
        public RegiaoConsumer(ILogger<RegiaoConsumer> logger, IRegiaoRepository regiaoRepository, IEstadoRepository estadoRepository)
        {
            _logger = logger;
            _regiaoRepository = regiaoRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task Consume(ConsumeContext<RegiaoConsumerDTO> context)
        {
            try
            {
                _logger.LogInformation($"Chegou no consumer");
                var regiao = context.Message;
                _logger.LogInformation($"Processando mensagem da região: {regiao.Estado}");

                Estado e = new Estado()
                {
                    siglaEstado = regiao.Estado.siglaEstado,
                    Nome = regiao.Estado.Nome
                };

                e = await _estadoRepository.AdicionarEstado(e);

                Regiao r = new Regiao()
                {
                    NumeroDDD = regiao.NumeroDDD,
                    EstadoId = e.Id,                   
                    IdLocalidadeAPI = regiao.Id
                };

                await _regiaoRepository.Adicionar(r);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem");
                throw;  
            }
        }
    }
}
