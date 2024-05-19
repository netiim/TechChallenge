using Core.DTOs;
using Core.Entidades;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace TemplateWebApiNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _token;
        private readonly ILogger<TokenController> _logger;
        public TokenController(ITokenService token, ILogger<TokenController> logger)
        {
            _token = token;
            _logger = logger;
        }

        /// <summary>
        /// Criar um token para um usuário com base no seu Username e senha
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
        [HttpPost]
        public IActionResult CriaToken([FromBody] UsuarioTokenDTO usuario)
        {
            try
            {
                string token = _token.GetToken(usuario);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized($"{ex.Message}");
            }            
        }
    }
}
