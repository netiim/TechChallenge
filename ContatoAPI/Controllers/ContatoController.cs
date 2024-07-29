using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using static Core.Entidades.Usuario;

namespace ContatoAPI.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos Contatos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do ContatoController.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        /// <param name="contatoService">O serviço de Contato.</param>
        /// <param name="mapper">O mapeador para conversão de objetos.</param>
        public ContatoController(ApplicationDbContext context, IContatoService contatoService, IMapper mapper)
        {
            _context = context;
            _contatoService = contatoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todos os contatos do banco.
        /// </summary>
        /// <returns>Uma lista de contatos.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de contatos.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("BuscarTodosContatos")]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var contatos = await _contatoService.ObterTodosAsync();
                List<ReadContatoDTO> result = _mapper.Map<List<ReadContatoDTO>>(contatos);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Obtém os contatos por DDD.
        /// </summary>
        /// <param name="ddd">O DDD a ser buscado.</param>
        /// <returns>Uma lista de contatos filtrados pelo DDD.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de contatos filtrados pelo DDD.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("BuscarPorDDD")]
        public async Task<IActionResult> ObterPorDdd(int ddd)
        {
            try
            {
                List<Contato> contatos = (List<Contato>)await _contatoService.FindAsync(c => c.Regiao.NumeroDDD == ddd);
                List<ReadContatoDTO> contatoDTO = _mapper.Map<List<ReadContatoDTO>>(contatos);
                return Ok(contatoDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Obtém um contato por ID.
        /// </summary>
        /// <param name="id">O ID do contato.</param>
        /// <returns>O contato correspondente ao ID.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna o contato.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var contato = await _contatoService.ObterPorIdAsync(id);
                if (contato == null)
                {
                    return NotFound();
                }

                ReadContatoDTO contatoDTO = _mapper.Map<ReadContatoDTO>(contato);
                return Ok(contatoDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        /// <summary>
        /// Adiciona um novo contato.
        /// </summary>
        /// <param name="contatoDTO">Os dados do novo contato.</param>
        /// <returns>O contato recém-criado.</returns>
        /// <response code="201">Se a operação foi bem-sucedida e retorna o contato criado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        /// <response code="401">Se o usuário não está autenticado.</response>
        /// <response code="403">Se o usuário não tem permissão para executar esta ação.</response>
        [HttpPost]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Adicionar([FromBody] CreateContatoDTO contatoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Contato contato = _mapper.Map<Contato>(contatoDTO);

                await _contatoService.AdicionarAsync(contato);
                return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, _mapper.Map<ReadContatoDTO>(contato));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Atualiza um contato existente no banco de dados.
        /// </summary>
        /// <param name="id">O ID do contato a ser atualizado.</param>
        /// <param name="contatoDTO">Os dados atualizados do contato.</param>
        /// <returns>Uma resposta indicando o resultado da operação.</returns>
        /// <response code="204">Se a operação foi bem-sucedida.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CreateContatoDTO contatoDTO)
        {
            try
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Remove um contato.
        /// </summary>
        /// <param name="id">O ID do contato a ser removido.</param>
        /// <returns>Uma resposta indicando o resultado da operação.</returns>
        /// <response code="204">Se a operação foi bem-sucedida.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var contatoExistente = await _contatoService.ObterPorIdAsync(id);
                if (contatoExistente == null)
                {
                    return NotFound();
                }

                await _contatoService.RemoverAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
