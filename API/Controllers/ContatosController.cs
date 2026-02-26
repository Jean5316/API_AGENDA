using API_AGENDA.DTOs;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API_AGENDA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContatosController : ControllerBase
    {
        //INJEÇÃO DE DEPENDÊNCIA PARA O REPOSITÓRIO
        private readonly IContatoService _service;
        private readonly ILogger<ContatosController> _logger;

        public ContatosController(IContatoService service, ILogger<ContatosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //função para pegar o id do usuário logado a partir dos claims do token JWT
        private int getUsuarioId()
        {
            var claim = User.FindFirst("id");
            return int.Parse(claim!.Value);
        }

        //função para pegar o nome do usuário logado a partir dos claims do token JWT
        private string getUsuarioNome()
        {
            var claim = User.FindFirst("Nome");
            return claim?.Value ?? "Desconhecido";
        }

        //GET: api/Contatos
        //TODOS CONTATOS ATIVOS
        //<summary>
        /// Retorna todos os contatos ativos do usuário logado.
        //</summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} solicitou listar todos os contatos", usuarioId, usuarioNome);
            var contatos = await _service.ListarContatos(usuarioId);

            return Ok(contatos);
        }

        //GET: api/Contatos/1
        //APENAS UM CONTATO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} solicitou contato por ID:{ContatoId}", usuarioId, usuarioNome, id);
            var contato = await _service.ListarContato(id, usuarioId);
            if (contato == null)
            {
                _logger.LogWarning("Contato ID:{ContatoId} não encontrado para Usuário ID:{UsuarioId} Nome:{UsuarioNome}", id, usuarioId, usuarioNome);
                return NotFound("Contato não encontrado");
            }
            return Ok(contato);
        }

        //APENAS FAVORITOS
        //GET: api/Contatos/favoritos
        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos()
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} solicitou lista de favoritos", usuarioId, usuarioNome);
            var contatos = await _service.ListarFavoritos(usuarioId);
            return Ok(contatos);
        }

        //BUSCAR POR NOME
        //GET: api/contatos/jean
        [HttpGet("buscar")]
        public async Task<ActionResult> GetNome([FromQuery] string nome)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} buscou contatos pelo nome:{Nome}", usuarioId, usuarioNome, nome);
            var contatos = await _service.ListarPorNome(nome, usuarioId);
            if (contatos == null)
            {
                _logger.LogWarning("Nenhum contato encontrado com nome:{Nome} para Usuário ID:{UsuarioId} Nome:{UsuarioNome}", nome, usuarioId, usuarioNome);
                return NotFound("Contato não encontrado");
            }

            return Ok(contatos);
        }

        //PAGINACAO
        [HttpGet("paginacao")]
        public async Task<IActionResult> Listar(int pagina = 1, int tamanhoPagina = 2)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} solicitou paginação - Página:{Pagina}, Tamanho:{TamanhoPagina}", usuarioId, usuarioNome, pagina, tamanhoPagina);
            var resultado = await _service.ListarPaginadoAsync(usuarioId, pagina, tamanhoPagina);

            return Ok(resultado);
        }

        //CRIANDO CONTATO
        //POST: api/Contatos
        [HttpPost]
        public async Task<IActionResult> CriarContato(ContatoCriarDto dto)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} está criando novo contato com nome:{Nome}", usuarioId, usuarioNome, dto.Nome);
            var contato = await _service.CriarContato(dto, usuarioId);
            _logger.LogInformation("Contato criado com sucesso - ID:{ContatoId} para Usuário ID:{UsuarioId} Nome:{UsuarioNome}", contato.Id, usuarioId, usuarioNome);
            return Ok(contato);
        }

        //ATUALIZANDO CONTATO
        //PUT: api/Contatos/1
        [HttpPut("AtualizarContato/{id}")]
        public async Task<IActionResult> AtualizarContato(int id, ContatoAtualizarDto dto)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogInformation("Usuário ID:{UsuarioId} Nome:{UsuarioNome} está atualizando contato ID:{ContatoId}", usuarioId, usuarioNome, id);
            var atualizado = await _service.AtualizarContato(dto, id, usuarioId);
            if (atualizado)
            {
                _logger.LogInformation("Contato ID:{ContatoId} atualizado com sucesso pelo Usuário ID:{UsuarioId} Nome:{UsuarioNome}", id, usuarioId, usuarioNome);
                return Ok("Contato atualizado com sucesso.");
            }
            _logger.LogWarning("Tentativa de atualizar contato ID:{ContatoId} falhou - Usuário ID:{UsuarioId} Nome:{UsuarioNome}", id, usuarioId, usuarioNome);
            return BadRequest("Contato não encontrado");
        }

        //DELETANDO CONTATO
        //DELETE: api/Contatos/1
        [HttpDelete("DeletarContato/{id}")]
        public async Task<IActionResult> DeletarContato(int id)
        {
            var usuarioId = getUsuarioId();
            var usuarioNome = getUsuarioNome();
            _logger.LogWarning("Usuário ID:{UsuarioId} Nome:{UsuarioNome} está deletando contato ID:{ContatoId}", usuarioId, usuarioNome, id);
            var deletado = await _service.DeletarContato(id, usuarioId);
            if (deletado)
            {
                _logger.LogWarning("Contato ID:{ContatoId} deletado com sucesso pelo Usuário ID:{UsuarioId} Nome:{UsuarioNome}", id, usuarioId, usuarioNome);
                return Ok("Usuario Deletado com sucesso!");
            }
            _logger.LogWarning("Tentativa de deletar contato ID:{ContatoId} falhou - Usuário ID:{UsuarioId} Nome:{UsuarioNome}", id, usuarioId, usuarioNome);

            return NotFound("Contato não encontrado");
        }
    }
}
