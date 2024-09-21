using Core.DTOs.RegiaoDTO;

namespace Core.DTOs.ContatoDTO;

public class ReadContatoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public ReadRegiaoDTO Regiao {  get; set; }
}
