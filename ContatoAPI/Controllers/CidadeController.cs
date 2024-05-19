using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateWebApiNet8.Controllers;
using TemplateWebApiNet8.Logging;

namespace ContatoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly ICidadeService _service;
        private readonly ILogger<CidadeController> _logger;

        public CidadeController(ICidadeService service, ILogger<CidadeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PreencherCidadesComDDD()
        {
            CustomLogger.Arquivo = true;
            _logger.LogInformation("Iniciando Função para preencher os DDDs");

            await _service.PreencherCidadesComDDD();
            return Ok();
        }
    }
}
