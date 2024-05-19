using Core.DTOs;
using Core.Entidades;
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

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var contatos = await _contatoService.ObterTodosAsync();
            return Ok(contatos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var contato = await _contatoService.ObterPorIdAsync(id);
            if (contato == null)
            {
                return NotFound();
            }
            return Ok(contato);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CreateContatoDTO contatoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Contato contato = MapearDTO(contatoDTO);

            await _contatoService.AdicionarAsync(contato);
            return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
        }

        private static Contato MapearDTO(CreateContatoDTO contatoDTO)
        {
            return new Contato()
            {
                Nome = contatoDTO.Nome,
                Email = contatoDTO.Email,
                Telefone = contatoDTO.Telefone,
                CidadeId = contatoDTO.CidadeId,
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreateContatoDTO contatoDTO)
        {
            var contatoExistente = await _contatoService.ObterPorIdAsync(id);
            if (contatoExistente == null)
            {
                return NotFound();
            }

            contatoExistente.Nome = contatoDTO.Nome;
            contatoExistente.Email = contatoDTO.Email;
            contatoExistente.Telefone = contatoDTO.Telefone;
            contatoExistente.CidadeId = contatoDTO.CidadeId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _contatoService.AtualizarAsync(contatoExistente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var contatoExistente = await _contatoService.ObterPorIdAsync(id);
            if (contatoExistente == null)
            {
                return NotFound();
            }

            await _contatoService.RemoverAsync(id);
            return NoContent();
        }
    }
}
