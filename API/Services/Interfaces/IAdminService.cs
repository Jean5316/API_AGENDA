using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Usuario;

namespace API.Services.Interfaces
{
    public interface IAdminService
    {
         Task<List<UsuarioResponseDto>> ListarUsuarios();
          Task<bool> AtualizarUsuario(UsuarioAtualizarDto dto, int id);
           Task<bool> DeletarUsuario(int id);
    }
}