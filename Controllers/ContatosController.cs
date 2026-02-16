using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
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
        private readonly IContatoRepository _repository;

        public ContatosController(IContatoRepository repository)
        {
            _repository = repository;
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
            var contatos = await _repository.GetAllContatosAsync(usuarioId);

            var response = contatos.Select(c => new ContatoResponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Categoria = c.Categoria,
                Favorito = c.Favorito
                
            });
            return Ok(response);
        }

        //GET: api/Contatos/1
        //APENAS UM CONTATO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = getUsuarioId();

            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);
            if (contato == null)
            {
                return NotFound();
            }
            var response = new ContatoResponseDto
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Email = contato.Email,
                Telefone = contato.Telefone,
                Categoria = contato.Categoria,
                Favorito = contato.Favorito
            };

            return Ok(response);
        }

        //APENAS FAVORITOS
        //GET: api/Contatos/favoritos
        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos()
        {
            var usuarioId = getUsuarioId();
            var contatos = await _repository.GetFavoritosAsync(usuarioId);
            var response = contatos.Select(c => new ContatoResponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Categoria = c.Categoria,
                Favorito = c.Favorito
            });
            return Ok(response);
        }

        //CRIANDO CONTATO
        //POST: api/Contatos
        [HttpPost]
        public async Task<IActionResult> CriarContato(ContatoCriarDto dto)
        {
            var usuarioId = getUsuarioId();

            var contato = new Contato
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Categoria = dto.Categoria,
                Favorito = dto.Favorito,
                UsuarioId = usuarioId
            };

            await _repository.AddContatoAsync(contato);

            return Ok(contato);
        }

        //ATUALIZANDO CONTATO
        //PUT: api/Contatos/1
        [HttpPut("AtualizarContato/{id}")]
        public async Task<IActionResult> AtualizarContato(int id, ContatoCriarDto dto)
        {
            var usuarioId = getUsuarioId();

            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);

            if (contato == null || !contato.Ativo)
            {
                return NotFound();
            }

            contato.Nome = dto.Nome;
            contato.Email = dto.Email;
            contato.Telefone = dto.Telefone;
            contato.Categoria = dto.Categoria;
            contato.Favorito = dto.Favorito;
            contato.DataAtualizacao = DateTime.Now;

            await _repository.UpdateContatoAsync(contato);

            return NoContent();
        }

        //DELETANDO CONTATO
        //DELETE: api/Contatos/1
        [HttpDelete("DeletarContato/{id}")]
        public async Task<IActionResult> DeletarContato(int id)
        {
            var usuarioId = getUsuarioId();

            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);

            if (contato == null)
            {
                return NotFound();
            }

            contato.Ativo = false;

            await _repository.DeleteContatoAsync(contato);

            return NoContent();
        }
    }
}