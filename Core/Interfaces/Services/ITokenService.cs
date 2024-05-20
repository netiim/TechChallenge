using Core.DTOs.UsuarioDTO;
using Core.Entidades;

namespace Core.Interfaces.Services;

public interface ITokenService
{
    string GetToken(UsuarioTokenDTO usuario);
}
