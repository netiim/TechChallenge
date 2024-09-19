using System.Reflection.PortableExecutable;

namespace Core.Entidades
{
    public class Contato : EntityBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int RegiaoId { get; set; }
        public virtual Regiao Regiao { get; set; }
    }
}
