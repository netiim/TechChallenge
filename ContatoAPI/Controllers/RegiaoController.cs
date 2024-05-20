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
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _service;
        private readonly ILogger<RegiaoController> _logger;

        public RegiaoController(IRegiaoService service, ILogger<RegiaoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PreencherCidadesComDDD()
        {
            CustomLogger.Arquivo = true;
            _logger.LogInformation("Iniciando Função para preencher os DDDs");

            await _service.PreencherRegioesComDDD();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var cidades = await _service.ObterTodosAsync();
            return Ok(cidades);
        }
    }
}
