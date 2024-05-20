using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core.Entidades.Usuario;

namespace ContatoAPI.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos Estados.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEstadoService _estadoService;
        /// <summary>
        /// Construtor do EstadoController.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        /// <param name="estadoService">O serviço de Estado.</param>
        public EstadoController(ApplicationDbContext context, IEstadoService estadoService)
        {
            _context = context;
            _estadoService = estadoService;
        }
        /// <summary>
        /// Popula a tabela de estados do Brasil.
        /// </summary>
        /// <returns>Um IActionResult indicando o resultado da operação.</returns>
        /// <response code="200">Se a operação foi bem-sucedida.</response>
        /// <response code="401">Se o usuário não está autenticado.</response>
        /// <response code="403">Se o usuário não tem permissão para executar esta ação.</response>
        [HttpPost]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> PreencherTabelaComEstadosBrasil()
        {
            try
            {
                await _estadoService.PreencherTabelaComEstadosBrasil();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
