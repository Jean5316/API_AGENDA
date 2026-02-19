using API_AGENDA.DTOs;
using API_AGENDA.Models;

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
    }
}
