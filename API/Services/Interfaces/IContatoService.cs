using API_AGENDA.DTOs;



namespace API_AGENDA.Services.Interfaces
{
    public interface IContatoService
    {
        Task<List<ContatoResponseDto>> ListarContatos(Guid usuarioId);
        Task<ContatoResponseDto> ListarContato(int id, Guid usuarioId);
        Task<List<ContatoResponseDto>> ListarFavoritos(Guid usuarioId);
        Task<ContatoResponseDto> CriarContato(ContatoCriarDto dto, Guid usuarioId);
        Task<bool> AtualizarContato(ContatoAtualizarDto dto, int id, Guid usuarioId );
        Task<bool> DeletarContato(int id, Guid usuarioId);
        Task<List<ContatoResponseDto>> ListarPorNome(string Nome, Guid usuarioId);
        Task<PaginacaoResponse<ContatoResponseDto>> ListarPaginadoAsync(Guid usuarioId, int pagina, int tamanhoPagina);

    }
}
