using Core.DTOs.ContatoDTO;
using MassTransit;

namespace ContatoWorker.Get
{
    public class GetContatoWorker
    {
        private readonly IRequestClient<GetContatosRequest> _requestClient;

        public GetContatoWorker(IRequestClient<GetContatosRequest> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<List<ReadContatoDTO>> ObterContatosAsync()
        {
            // Cria uma solicitação de requisição de contatos
            var response = await _requestClient.GetResponse<ContatosResponse>(new GetContatosRequest());

            // Retorna a lista de contatos da resposta
            return response.Message.Contatos;
        }
    }
}
