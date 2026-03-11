using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.Repository.Interfaces;
using API_AGENDA.Services.Interfaces;
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



        public async Task<bool> AtualizarContato(ContatoAtualizarDto dto, int id, Guid usuarioId)
        {
            var contato = await _repository.GetByIdAsync(id, usuarioId);

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

            await _repository.UpdateAsync(contato);
            return true;


        }

        public async Task<ContatoResponseDto> CriarContato(ContatoCriarDto dto, Guid usuarioId)
        {
            
            //converte para entidade
            var contato = new Contato
            {
                Nome = dto.Nome!,
                Email = dto.Email!,
                Telefone = dto.Telefone!,
                Categoria = dto.Categoria!,
                Favorito = dto.Favorito,
                UsuarioId = usuarioId
            };


            await _repository.AddAsync(contato);
            //converte para dto
            return new ContatoResponseDto
            {
                Id = contato.Id!,
                Nome = contato.Nome!,
                Telefone = contato.Telefone!,
                Categoria = contato.Categoria!,
                Favorito = contato.Favorito,

            };


        }

        public async Task<bool> DeletarContato(int id, Guid usuarioId)
        {
            var contato = await _repository.GetByIdAsync(id, usuarioId);

            if (contato == null)
            {
                return false;

            }

            await _repository.DeleteAsync(contato);
            return true;


        }

        public async Task<ContatoResponseDto> ListarContato(int id, Guid usuarioId)
        {
            var contato = await _repository.GetByIdAsync(id, usuarioId);
            if (contato == null)
            {
                return null!;
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

        public async Task<List<ContatoResponseDto>> ListarContatos(Guid usuarioId)
        {
            var contatos = await _repository.GetAllAsync(usuarioId);

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

        public async Task<List<ContatoResponseDto>> ListarFavoritos(Guid usuarioId)
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

        public async Task<PaginacaoResponse<ContatoResponseDto>> ListarPaginadoAsync(Guid usuarioId, int pagina, int tamanhoPagina)
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

        public async Task<List<ContatoResponseDto>> ListarPorNome(string Nome, Guid usuarioId)
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
