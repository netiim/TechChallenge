using Core.Interfaces;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> PreencherTabelaComEstadosBrasil()
        {
            await _estadoService.PreencherTabelaComEstadosBrasil();
            return Ok();
        }
    }
}
