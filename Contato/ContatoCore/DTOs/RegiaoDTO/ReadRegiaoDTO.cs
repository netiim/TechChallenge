using Core.DTOs.EstadoDTO;

namespace Core.DTOs.RegiaoDTO
{
    public class ReadRegiaoDTO
    {
        public int numeroDDD { get; set; }
        public int EstadoId { get; set; }
        public ReadEstadoDTO Estado { get; set; }
    }
}
