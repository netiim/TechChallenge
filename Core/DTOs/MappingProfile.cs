using AutoMapper;
using Core.DTOs.CidadeDTO;
using Core.DTOs.ContatoDTO;
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
            CreateMap<Cidade, ReadCidadeDTO>()
                .ReverseMap();           
        }
    }
}
