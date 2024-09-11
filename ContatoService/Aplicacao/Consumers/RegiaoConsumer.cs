using Castle.Core.Logging;
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
                var regiao = context.Message;


                Estado estadoExistente = await _estadoRepository.BuscarEstadoPorSigla(regiao.siglaEstado);


                if (estadoExistente is null)
                    throw new ArgumentNullException("Não existe um estado para essa região");


                _logger.LogInformation($"Adicionou o estado");
                Regiao r = new Regiao()
                {
                    NumeroDDD = regiao.NumeroDDD,
                    EstadoId = estadoExistente.Id,                   
                    IdLocalidadeAPI = regiao.Id
                };

                await _regiaoRepository.Adicionar(r);
                _logger.LogInformation($"Adicionou o regiao");
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
