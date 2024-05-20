using Core.DTOs.RegiaoDTO;

namespace Core.DTOs.ContatoDTO;

public class ReadContatoDTO
{   
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Telefone { get; set; }
    public ReadRegiaoDTO Regiao {  get; set; }
}
