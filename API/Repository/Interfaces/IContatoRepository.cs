using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.DTOs;
using API_AGENDA.Models;

namespace API_AGENDA.Repository.Interfaces
{
    public interface IContatoRepository
    {
        Task<List<Contato>> GetAllContatosAsync(int usuarioId);
        Task<List<Contato>> GetFavoritosAsync(int usuarioId);
        Task<Contato?> GetContatoByIdAsync(int id, int usuarioId);
        Task AddContatoAsync(Contato contato);
        Task UpdateContatoAsync(Contato contato);
        Task DeleteContatoAsync(Contato contato);
        Task<List<Contato>> GetName(string Nome, int usuarioId);

        Task<PaginacaoResponse<Contato>> ListaPaginadoAsync(int usuarioId, int pagina, int tamanhoPagina); 


        
    }
}