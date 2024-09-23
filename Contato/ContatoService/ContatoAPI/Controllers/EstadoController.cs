using AutoMapper;
using Core.DTOs.EstadoDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContatoAPI.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos Estados.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoService _estadoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// Construtor do EstadoController.
        /// </summary>
        /// <param name="estadoService">O serviço de Estado.</param>
        /// <param name="mapper">O mapeador para conversão de objetos.</param>
        public EstadoController( IEstadoService estadoService, IMapper mapper)
        {
            _estadoService = estadoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todos os estados.
        /// </summary>
        /// <returns>Uma lista de estados.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de estados.</response>
        /// <response code="400">Se aconteceu algum problema com a operação.</response>
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var regiao = await _estadoService.ObterTodos();
                List<ReadEstadoDTO> regiaoDTO = _mapper.Map<List<ReadEstadoDTO>>(regiao);
                return Ok(regiaoDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
