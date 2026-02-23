using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.ModelViews;

namespace API_AGENDA.Services.Interfaces
{
    public interface IContatoService
    {
        Task<List<ContatoResponseDto>> ListarContatos(int usuarioId);
        Task<ContatoResponseDto> ListarContato(int id, int usuarioId);
        Task<List<ContatoResponseDto>> ListarFavoritos(int usuarioId);
        Task<ContatoResponseDto> CriarContato(ContatoCriarDto dto, int usuarioId);
        Task<bool> AtualizarContato(ContatoAtualizarDto dto, int id, int usuarioId );
        Task<bool> DeletarContato(int id, int usuarioId);
        Task<List<ContatoResponseDto>> ListarPorNome(string Nome, int usuarioId);
        Task<PaginacaoResponse<ContatoResponseDto>> ListarPaginadoAsync(int usuarioId, int pagina, int tamanhoPagina);
        
    }
}
