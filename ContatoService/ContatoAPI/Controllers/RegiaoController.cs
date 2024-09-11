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
    /// <summary>
    /// Controller responsável pelas operações relacionadas às Regiões.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _service;
        private readonly ILogger<RegiaoController> _logger;
        private readonly IMapper _mapper;
        /// <summary>
        /// Construtor do RegiaoController.
        /// </summary>
        /// <param name="service">O serviço de Região.</param>
        /// <param name="logger">O logger para logging de informações.</param>
        /// <param name="mapper">O mapeador para conversão de objetos.</param>
        public RegiaoController(IRegiaoService service, ILogger<RegiaoController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Obtém todas as regiões.
        /// </summary>
        /// <returns>Uma lista de regiões.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de regiões.</response>
        /// <response code="400">Se aconteceu algum problema com a operação.</response>
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var regiao = await _service.ObterTodosAsync();
                List<ReadRegiaoDTO> regiaoDTO = _mapper.Map<List<ReadRegiaoDTO>>(regiao);
                return Ok(regiaoDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }           
        }
    }
}
