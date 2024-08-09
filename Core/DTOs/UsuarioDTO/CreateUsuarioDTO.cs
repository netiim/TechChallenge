using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Entidades.Usuario;

namespace Core.DTOs.UsuarioDTO;

public class CreateUsuarioDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public PerfilUsuario Perfil { get; set; }
}
