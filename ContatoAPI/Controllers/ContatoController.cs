using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContatoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IContatoService _contatoService;

        public ContatoController(ApplicationDbContext context, IContatoService contatoService)
        {
            _context = context;
            _contatoService = contatoService;
        }

        [HttpPost]
        public async Task<IActionResult> PreencherTabelaComEstadosBrasil()
        {
            //await _contatoService.PreencherTabelaComEstadosBrasil();
            return Ok();
        }
    }
}
