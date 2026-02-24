using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Models;

namespace API.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllUsuariosAsync();
         Task UpdateUsuarioAsync(Usuario usuario);
         Task DeleteUsuarioAsync(Usuario usuario);
         Task<Usuario?> GetUsuarioByIdAsync(int id);
    }
}