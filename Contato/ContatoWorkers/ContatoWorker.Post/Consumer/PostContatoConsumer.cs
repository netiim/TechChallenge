using AutoMapper;
using Azure;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.DTOs.ContatoDTO;
using Core.Entidades;
using Core.Interfaces.Services;
using MassTransit;

namespace ContatoWorker.Post.Consumers;

public class PostContatoConsumer : IConsumer<PostContatosRequest>
{
    private readonly IContatoService _contatoService;
    private readonly IMapper _mapper;

    public PostContatoConsumer(IContatoService contatoService, IMapper mapper)
    {
        _contatoService = contatoService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<PostContatosRequest> context)
    {
        try
        {
            var contatoDTO = context.Message.CreateContatoDTO;
            Contato contato = _mapper.Map<Contato>(contatoDTO);

            await _contatoService.AdicionarAsync(contato);

            await context.RespondAsync<ContatoResponse>(new ContatoResponse { Contato = _mapper.Map<ReadContatoDTO>(contato) });
        }
        catch (Exception e)
        {
            await context.RespondAsync<ContatoErroResponse>(new ContatoErroResponse { MensagemErro = $"{e.Message}" });
            throw;
        }
        
    }
}
