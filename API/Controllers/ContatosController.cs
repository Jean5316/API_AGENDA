using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.ModelViews;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace API_AGENDA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContatosController : ControllerBase
    {
        //INJEÇÃO DE DEPENDÊNCIA PARA O REPOSITÓRIO
        private readonly IContatoService _service;

        public ContatosController(IContatoService service)
        {
            _service = service;
        }

        //função para pegar o id do usuário logado a partir dos claims do token JWT
        private int getUsuarioId()
        {
            var claim = User.FindFirst("id");
            return int.Parse(claim!.Value);
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
            var contatos = await _service.ListarContatos(usuarioId);

            return Ok(contatos);


        }

        //GET: api/Contatos/1
        //APENAS UM CONTATO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = getUsuarioId();
            var contato = await _service.ListarContato(id, usuarioId);
            if (contato == null)
            {
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

            var contatos = await _service.ListarFavoritos(usuarioId);
            return Ok(contatos);

        }

        //BUSCAR POR NOME
        //GET: api/contatos/jean
        [HttpGet("buscar")]
        public async Task<ActionResult> GetNome([FromQuery] string nome)
        {
            var ususarioId = getUsuarioId();
            var contatos = await _service.ListarPorNome(nome, ususarioId);
            if (contatos == null)
            {
                return NotFound("Contato não encontrado");
            }

            return Ok(contatos);

        }

        //PAGINACAO
        [HttpGet("paginacao")]
        public async Task<IActionResult> Listar(int pagina = 1, int tamanhoPagina = 2)
        {
            var usuarioId = getUsuarioId();
            var resultado = await _service.ListarPaginadoAsync(usuarioId, pagina, tamanhoPagina);

            return Ok(resultado);
        }



        //CRIANDO CONTATO
        //POST: api/Contatos
        [HttpPost]
        public async Task<IActionResult> CriarContato(ContatoCriarDto dto)
        {
            var usuarioId = getUsuarioId();


            var contato = await _service.CriarContato(dto, usuarioId);
            return Ok(contato);


        }

        //ATUALIZANDO CONTATO
        //PUT: api/Contatos/1
        [HttpPut("AtualizarContato/{id}")]
        public async Task<IActionResult> AtualizarContato(int id, ContatoAtualizarDto dto)
        {
            var usuarioId = getUsuarioId();
            var atualizado = await _service.AtualizarContato(dto, id, usuarioId);
            if (atualizado)
            {
               return Ok("Contato atualizado com sucesso.");
            }
             return BadRequest("Contato não encontrado");

        }

        //DELETANDO CONTATO
        //DELETE: api/Contatos/1
        [HttpDelete("DeletarContato/{id}")]
        public async Task<IActionResult> DeletarContato(int id)
        {
            var usuarioId = getUsuarioId();

            var deletado = await _service.DeletarContato(id, usuarioId);
            if (deletado)
            {
                return Ok("Usuario Deletado com sucesso!");
            }

            return NotFound("Contato não encontrado");


        }
    }
}