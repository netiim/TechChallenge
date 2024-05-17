using Core.Interfaces;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContatoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICidadeService _service;

        public CidadeController(ApplicationDbContext context, ICidadeService service)
        {
            _context = context;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PreencherCidadesComDDD()
        {
            await _service.PreencherCidadesComDDD();
            return Ok();
        }
    }
}
