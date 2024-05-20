using AutoMapper;
using Core.DTOs.ContatoDTO;
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
        private readonly IMapper _mapper;

        public ContatoController(ApplicationDbContext context, IContatoService contatoService, IMapper mapper)
        {
            _context = context;
            _contatoService = contatoService;
            _mapper = mapper;
        }

        [HttpGet("BuscarTodosContatos")]
        public async Task<IActionResult> ObterTodos()
        {
            var contatos = await _contatoService.ObterTodosAsync();
            List<ReadContatoDTO> result = _mapper.Map<List<ReadContatoDTO>>(contatos);
            return Ok(result);
        }

        [HttpGet("BuscarPorDDD")]
        public async Task<IActionResult> ObterPorDdd(int ddd)
        {
            var contatos = await _contatoService.FindAsync(c => c.Regiao.numeroDDD == ddd);
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
            try
            {
                Contato contato = _mapper.Map<Contato>(contatoDTO);

                await _contatoService.AdicionarAsync(contato);
                return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
