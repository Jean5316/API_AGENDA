using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.ModelViews;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API_AGENDA.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _repository;

        public ContatoService(IContatoRepository repository)
        {
            _repository = repository;
        }



        public async Task<bool> AtualizarContato(ContatoAtualizarDto dto, int id, int usuarioId)
        {
            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);

            if (contato == null || !contato.Ativo)
            {
                return false;
            }

            contato.Nome = dto.Nome;
            contato.Email = dto.Email;
            contato.Telefone = dto.Telefone;
            contato.Categoria = dto.Categoria;
            contato.Favorito = dto.Favorito;
            contato.DataAtualizacao = DateTime.Now;

            await _repository.UpdateContatoAsync(contato);
            return true;


        }

        public async Task<ContatoResponseDto> CriarContato(ContatoCriarDto dto, int usuarioId)
        {
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

            return new ContatoResponseDto
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Telefone = contato.Telefone,
                Categoria = contato.Categoria,
                Favorito = contato.Favorito,

            };


        }

        public async Task<bool> DeletarContato(int id, int usuarioId)
        {
            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);

            if (contato == null)
            {
                return false;

            }

            await _repository.DeleteContatoAsync(contato);
            return true;


        }

        public async Task<ContatoResponseDto> ListarContato(int id, int usuarioId)
        {
            var contato = await _repository.GetContatoByIdAsync(id, usuarioId);
            if (contato == null)
            {
                return null;
            }
            return new ContatoResponseDto
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Email = contato.Email,
                Telefone = contato.Telefone,
                Categoria = contato.Categoria,
                Favorito = contato.Favorito
            };


        }

        public async Task<List<ContatoResponseDto>> ListarContatos(int usuarioId)
        {
            var contatos = await _repository.GetAllContatosAsync(usuarioId);

            return contatos.Select(c => new ContatoResponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Categoria = c.Categoria,
                Favorito = c.Favorito

            }).ToList();

        }

        public async Task<List<ContatoResponseDto>> ListarFavoritos(int usuarioId)
        {
            var contatos = await _repository.GetFavoritosAsync(usuarioId);
            return contatos.Select(c => new ContatoResponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Categoria = c.Categoria,
                Favorito = c.Favorito
            }).ToList();

        }

        public async Task<PaginacaoResponse<ContatoResponseDto>> ListarPaginadoAsync(int usuarioId, int pagina, int tamanhoPagina)
        {
            if (pagina <= 0) pagina = 1;

            if (tamanhoPagina <= 0 || tamanhoPagina > 50) tamanhoPagina = 10;

            var resultado = await _repository.ListaPaginadoAsync(usuarioId, pagina, tamanhoPagina);

            return new PaginacaoResponse<ContatoResponseDto>
            {
                Pagina = resultado.Pagina,
                TamanhoPagina = resultado.TamanhoPagina,
                TotalRegistros = resultado.TotalRegistros,
                TotalPaginas = resultado.TotalPaginas,
                Dados = resultado.Dados.Select(c => new ContatoResponseDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    Categoria = c.Categoria,
                    Favorito = c.Favorito
                }).ToList()
            };
        }

        public async Task<List<ContatoResponseDto>> ListarPorNome(string Nome, int usuarioId)
        {
            var contatos = await _repository.GetName(Nome, usuarioId);
            return contatos.Select(c => new ContatoResponseDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Categoria = c.Categoria,
                Favorito = c.Favorito
            }).ToList();
        }


    }
}
