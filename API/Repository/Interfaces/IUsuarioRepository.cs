using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Models;
using API_AGENDA.Repository.Interfaces;

namespace API.Repository.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // Task<List<Usuario>> GetAllUsuariosAsync();
        // Task UpdateUsuarioAsync(Usuario usuario);
        // Task DeleteUsuarioAsync(Usuario usuario);
        // Task<Usuario?> GetUsuarioByIdAsync(Guid id);
        Task<Usuario> GetByIdAsync(Guid id);
        Task<IEnumerable<Usuario>> GetAllAsync();
    }
}