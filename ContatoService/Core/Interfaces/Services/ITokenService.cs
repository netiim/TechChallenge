using Core.DTOs.UsuarioDTO;
using Core.Entidades;

namespace Core.Interfaces.Services;

public interface ITokenService
{
    Task<string> GetToken(UsuarioTokenDTO usuario);
    Task CriarUsuario(Usuario usuario);
}
