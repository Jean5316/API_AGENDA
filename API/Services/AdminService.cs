using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Usuario;
using API.Repository.Interfaces;
using API.Services.Interfaces;

namespace API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public AdminService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<bool> AtualizarUsuario(UsuarioAtualizarDto dto, Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
              return false;
            }

            usuario.Name = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Role = dto.Role;
            usuario.Ativo = dto.Ativo;

            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }

        public async Task<bool> DeletarUsuario(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario == null) return false;

            await _usuarioRepository.DeleteAsync(usuario);
            return true;
        }

        public async Task<List<UsuarioResponseDto>> ListarUsuarios()
        {
           var usuarios = await _usuarioRepository.GetAllAsync();
           return usuarios.Select(u => new UsuarioResponseDto
           {
               Id = u.Id,
               Nome = u.Name,
               Email = u.Email,
               Role = u.Role,
               Ativo = u.Ativo
           }).ToList();

        }

        //ToDo: Implementar método para filtrar usuários ativos/inativos, nome
    }
}