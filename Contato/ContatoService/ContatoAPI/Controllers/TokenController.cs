using Aplicacao.Services;
using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.Entidades.Usuario;

namespace TemplateWebApiNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _token;
        private readonly ILogger<TokenController> _logger;
        private readonly IMapper _mapper;
        public TokenController(ITokenService token, ILogger<TokenController> logger, IMapper mapper)
        {
            _token = token;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Criar um token para um usuário caso exista um usuário e senha com esses valores no banco
        /// </summary>
        /// <param name="usuario">Recebe um objeto do tipo UsuarioTemplate</param>
        /// <remarks>
        /// Não é necessario informar o ID
        /// </remarks>
        /// <returns>Retorna um token de autenticação no formato JWT
        /// que poderá ser utilizado para requisitar alguns EndPoints
        /// </returns>
        /// <response code="200">Sucesso ao gerar o token</response>
        /// <response code="401">Usuário ou senha inválidos</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpPost("CriarToken")]
        public async Task<IActionResult> CriaToken([FromBody] UsuarioTokenDTO usuario)
        {
            try
            {
                string token = await _token.GetToken(usuario);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized($"{ex.Message}");
            }
        }

        /// <summary>
        /// Criar um usuário no banco de dados.
        /// </summary>
        /// <param name="usuario">Recebe um objeto do tipo Createusuario</param>
        /// <remarks>
        /// Como possiveis perfis temos:
        /// PerfilUsuario
        ///{
        /// Administrador = 1,
        ///  Visitante = 2,
        ///  Usuario = 3
        ///  }
        /// </remarks>
        /// <returns>Retorna um token de autenticação no formato JWT
        /// que poderá ser utilizado para requisitar alguns EndPoints
        /// </returns>
        /// <response code="200">Sucesso ao gerar o token</response>
        /// <response code="401">Usuário ou senha inválidos</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpPost("criar-usuario")]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Criausuario([FromBody] CreateUsuarioDTO usuarioDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Usuario usuario = _mapper.Map<Usuario>(usuarioDTO);

                await _token.CriarUsuario(usuario);

                UsuarioTokenDTO usuarioToken = _mapper.Map<UsuarioTokenDTO>(usuario);

                string token = await _token.GetToken(usuarioToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized($"{ex.Message}");
            }

        }
    }
}
