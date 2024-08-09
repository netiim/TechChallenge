using Core.Entidades;

namespace Core.Interfaces.Repository;

public interface ITokenRepository
{
    Task Adicionar(Usuario usuario);
    Task<IEnumerable<Usuario>> ListarUsuarios();
}
