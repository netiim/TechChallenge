using Core.DTOs.UsuarioDTO;
using Core.Entidades;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Core.Entidades.Usuario;

namespace TemplateWebApiNet8.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        protected readonly ITokenRepository _tokenRepository;
        public List<Usuario> Usuarios = new List<Usuario>() { new Usuario() {Username = "netim", Password = "123456", Perfil = PerfilUsuario.Administrador }
        };

        public TokenService(IConfiguration configuration, ITokenRepository tokenRepository)
        {
            this.configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        public async Task CriarUsuario(Usuario usuario)
        {
            await _tokenRepository.Adicionar(usuario);
        }

        public async Task<string> GetToken(UsuarioTokenDTO usuario)
        {
            Usuario? usuarioExiste = _tokenRepository.ListarUsuarios().Result.ToList().FirstOrDefault(usu => usu.Username.Equals(usuario.Username)
                                && usu.Password.Equals(usuario.Password));

            if (usuarioExiste == null)
                throw new Exception("Usuário ou Senha inválidos");

            JwtSecurityTokenHandler tokenHandler = new();
            //Recuperar chave feita no app.settings se a chave for muito pequena da erro tem que ser mais de 256 bits usar gerador
            byte[] chaveCriptografada = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));

            SecurityTokenDescriptor tokenPropriedades = new()
            {
                //Atributos que o meu token vai ter
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioExiste.Username),
                    new Claim(ClaimTypes.Role, usuarioExiste.Perfil.ToString()),
                }) ,
                //Definir tempo de expiração para token
                Expires = DateTime.UtcNow.AddHours(8),

                //Adicionar chave de criptografia
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chaveCriptografada),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenPropriedades);

            return tokenHandler.WriteToken(token);

        }
    }
}
