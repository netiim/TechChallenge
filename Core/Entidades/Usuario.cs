using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades;

public class Usuario : EntityBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public PerfilUsuario Perfil { get; set; }
    public enum PerfilUsuario
    {
        Administrador = 1,
        Visitante = 2,
        Usuario = 3
    }
    public static class Roles
    {
        public const string Administrador = nameof(PerfilUsuario.Administrador);
        public const string Visitante = nameof(PerfilUsuario.Visitante);
        public const string Usuario = nameof(PerfilUsuario.Usuario);
    }
}
