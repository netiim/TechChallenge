using AutoMapper;
using Core.Contratos.Contatos;
using Core.Contratos.Request;
using Core.Contratos.Response;
using Core.DTOs.ContatoDTO;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace ContatoAPI.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos Contatos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<GetContatosRequest> _requestGetClient;
        private readonly IRequestClient<PostContatosRequest> _requestPostClient;
        private readonly IRequestClient<DeleteContatoRequest> _requestDeleteClient;
        private readonly IRequestClient<PutContatoRequest> _requestUpdateClient;
        private readonly ILogger<ContatoController> logger;
        public static readonly Counter publishFaultCounter = Metrics.CreateCounter(
    "mt_publish_fault_total",
    "Total number of publish faults",
    new CounterConfiguration
    {
        LabelNames = new[] { "message_type" }
    }
);
        /// <summary>
        /// Construtor do ContatoController.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        /// <param name="contatoService">O serviço de Contato.</param>
        /// <param name="mapper">O mapeador para conversão de objetos.</param>
        public ContatoController(IMapper mapper, IPublishEndpoint publishEndpoint, IRequestClient<GetContatosRequest> requestClient, ILogger<ContatoController> logger, IRequestClient<PostContatosRequest> requestPostClient, IRequestClient<DeleteContatoRequest> requestDeleteClient, IRequestClient<PutContatoRequest> requestUpdateClient)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _requestGetClient = requestClient;
            this.logger = logger;
            _requestPostClient = requestPostClient;
            _requestDeleteClient = requestDeleteClient;
            _requestUpdateClient = requestUpdateClient;
        }

        /// <summary>
        /// Obtém todos os contatos do banco .
        /// </summary>
        /// <returns>Uma lista de contatos.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de contatos.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("BuscarTodosContatos-TesteKubernetes")]
        public async Task<IActionResult> ObterTodos()
        {
            var response = await _requestGetClient.GetResponse<ContatosResponse, ContatoErroResponse>(new GetContatosRequest(), timeout: TimeSpan.FromSeconds(30));

            if (response.Is(out Response<ContatoErroResponse> erro))
            {
                return BadRequest(erro.Message.MensagemErro);
            }

            response.Is(out Response<ContatosResponse> result);

            return Ok(result.Message.Contatos);
        }

        /// <summary>
        /// Obtém os contatos por DDD.
        /// </summary>
        /// <param name="ddd">O DDD a ser buscado.</param>
        /// <returns>Uma lista de contatos filtrados pelo DDD.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna a lista de contatos filtrados pelo DDD.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("BuscarPorDDD")]
        public async Task<IActionResult> ObterPorDdd(int ddd)
        {
            var response = await _requestGetClient.GetResponse<ContatosResponse, ContatoErroResponse, ContatoNotFound>(new GetContatosRequest { NumeroDDD = ddd }, timeout: TimeSpan.FromSeconds(30));

            if (!response.Is(out Response<ContatosResponse> result))
            {
                if (response.Is(out Response<ContatoNotFound> notFound))
                {
                    return NotFound(notFound.Message.Mensagem);
                }
                else if (response.Is(out Response<ContatoErroResponse> erro))
                {
                    return BadRequest(erro.Message.MensagemErro);
                }
            }

            return Ok(result.Message.Contatos);
        }

        /// <summary>
        /// Obtém um contato por ID.
        /// </summary>
        /// <param name="id">O ID do contato.</param>
        /// <returns>O contato correspondente ao ID.</returns>
        /// <response code="200">Se a operação foi bem-sucedida e retorna o contato.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var response = await _requestGetClient.GetResponse<ContatosResponse, ContatoErroResponse, ContatoNotFound>(new GetContatosRequest { ContatoId = id }, timeout: TimeSpan.FromSeconds(30));
            try
            {
                if (response.Is(out Response<ContatoNotFound> notFound))
                {
                    return NotFound(notFound.Message.Mensagem);
                }
                else if (response.Is(out Response<ContatoErroResponse> erro))
                {
                    throw new Exception(erro.Message.MensagemErro);
                }

                response.Is(out Response<ContatosResponse> result);
                return Ok(result.Message.Contatos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        /// <summary>
        /// Adiciona um novo contato.
        /// </summary>
        /// <param name="contatoDTO">Os dados do novo contato.</param>
        /// <returns>O contato recém-criado.</returns>
        /// <response code="201">Se a operação foi bem-sucedida e retorna o contato criado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        /// <response code="401">Se o usuário não está autenticado.</response>
        /// <response code="403">Se o usuário não tem permissão para executar esta ação.</response>
        [HttpPost]
        //[Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Adicionar([FromBody] CreateContatoDTO contatoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _requestPostClient.GetResponse<ContatoResponse, ContatoErroResponse>(new PostContatosRequest { CreateContatoDTO = contatoDTO }, timeout: TimeSpan.FromSeconds(30));

                if (response.Is(out Response<ContatoErroResponse> erro))
                {
                    throw new Exception(erro.Message.MensagemErro);
                }
                response.Is(out Response<ContatoResponse> result);

                return Ok(result.Message.Contato);
            }
            catch (Exception e)
            {
                publishFaultCounter.WithLabels("ContatoErroResponse").Inc();
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Atualiza um contato existente no banco de dados.
        /// </summary>
        /// <param name="id">O ID do contato a ser atualizado.</param>
        /// <param name="contatoDTO">Os dados atualizados do contato.</param>
        /// <returns>Uma resposta indicando o resultado da operação.</returns>
        /// <response code="204">Se a operação foi bem-sucedida.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] PutContatoDTO contatoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _requestUpdateClient.GetResponse<ContatoResponse, ContatoNotFound, ContatoErroResponse>(new PutContatoRequest { ContatoDTO = contatoDTO }, timeout: TimeSpan.FromSeconds(30));

                if (response.Is(out Response<ContatoNotFound> notFound))
                {
                    return NotFound(notFound.Message.Mensagem);
                }
                else if (response.Is(out Response<ContatoErroResponse> erro))
                {
                    throw new Exception(erro.Message.MensagemErro);
                }
                response.Is(out Response<ContatoResponse> result);
                return Ok(result.Message.Contato);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Remove um contato.
        /// </summary>
        /// <param name="id">O ID do contato a ser removido.</param>
        /// <returns>Uma resposta indicando o resultado da operação.</returns>
        /// <response code="204">Se a operação foi bem-sucedida.</response>
        /// <response code="404">Se o contato não for encontrado.</response>
        /// <response code="400">Se houve um erro na solicitação.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var response = await _requestDeleteClient.GetResponse<ContatoSucessResponse, ContatoNotFound, ContatoErroResponse>(new DeleteContatoRequest { Id = id }, timeout: TimeSpan.FromSeconds(30));

                if (response.Is(out Response<ContatoNotFound> notFound))
                {
                    return NotFound(notFound.Message.Mensagem);
                }
                else if (response.Is(out Response<ContatoErroResponse> erro))
                {
                    throw new Exception(erro.Message.MensagemErro);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
