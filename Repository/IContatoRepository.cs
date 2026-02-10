using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Models;

namespace API_AGENDA.Controllers
{
    public interface IContatoRepository
    {
        Task<List<Contato>> GetAllContatosAsync();
        Task<List<Contato>> GetFavoritosAsync();
        Task<Contato?> GetContatoByIdAsync(int id);
        Task AddContatoAsync(Contato contato);
        Task UpdateContatoAsync(Contato contato);
        Task DeleteContatoAsync(Contato contato);
        
    }
}