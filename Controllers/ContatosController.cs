using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : ControllerBase
    {
        //INJEÇÃO DE DEPENDÊNCIA PARA O REPOSITÓRIO
        private readonly IContatoService _service;

        public ContatosController(IContatoService service)
        {
            _service = service;
        }

        private int getUsuarioId()
        {
            var claim = User.FindFirst("id");
            return int.Parse(claim!.Value);
        }


        
        //GET: api/Contatos 
        //TODOS CONTATOS ATIVOS
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
            if(contato == null)
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
            if (!atualizado)
            {
                return NotFound("Contato não encontrado");
            }
            return NoContent();
            
        }

        //DELETANDO CONTATO
        //DELETE: api/Contatos/1
        [HttpDelete("DeletarContato/{id}")]
        public async Task<IActionResult> DeletarContato(int id)
        {
            var usuarioId = getUsuarioId();

            var deletado = await _service.DeletarContato(id, usuarioId);
            if (!deletado)
            {
                return NotFound("Contato não encontrado");
            }

            return NoContent();

            
        }
    }
}