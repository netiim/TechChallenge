using AutoMapper;
using Core.DTOs.ContatoDTO;
using Core.DTOs.EstadoDTO;
using Core.DTOs.RegiaoDTO;
using Core.Entidades;

namespace Core.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contato, CreateContatoDTO>()
                .ReverseMap();
            CreateMap<Contato, ReadContatoDTO>()
                .ReverseMap();  
            CreateMap<Regiao, ReadRegiaoDTO>()
                .ReverseMap();    
            CreateMap<Estado, ReadEstadoDTO>()
                .ReverseMap();           
        }
    }
}
