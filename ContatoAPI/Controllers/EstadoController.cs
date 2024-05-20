using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core.Entidades.Usuario;

namespace ContatoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEstadoService _estadoService;

        public EstadoController(ApplicationDbContext context, IEstadoService estadoService)
        {
            _context = context;
            _estadoService = estadoService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> PreencherTabelaComEstadosBrasil()
        {
            await _estadoService.PreencherTabelaComEstadosBrasil();
            return Ok();
        }
    }
}
