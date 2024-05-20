using AutoMapper;
using Core.DTOs.RegiaoDTO;
using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateWebApiNet8.Controllers;
using TemplateWebApiNet8.Logging;
using static Core.Entidades.Usuario;

namespace ContatoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _service;
        private readonly ILogger<RegiaoController> _logger;
        private readonly IMapper _mapper;

        public RegiaoController(IRegiaoService service, ILogger<RegiaoController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrador)]
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
            var regiao = await _service.ObterTodosAsync();
            List<ReadRegiaoDTO> regiaoDTO = _mapper.Map<List<ReadRegiaoDTO>>(regiao);
            return Ok(regiaoDTO);
        }
    }
}
