using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using API.Repository.Interfaces;
using API_AGENDA.Context;
using API_AGENDA.Models;
using API_AGENDA.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AgendaContext context) : base(context)
        {
            
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.Select(u => new Usuario
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Ativo = u.Ativo
            }).ToListAsync();
        }

        // public async Task DeleteUsuarioAsync(Usuario usuario)
        // {
        //     _context.Usuarios.Remove(usuario);
        //     await _context.SaveChangesAsync();
        // }
        //
        // public async Task<List<Usuario>> GetAllUsuariosAsync()
        // {
        //     return await _context.Usuarios.Select(u => new Usuario
        //     {
        //         Id = u.Id,
        //         Name = u.Name,
        //         Email = u.Email,
        //         Role = u.Role,
        //         Ativo = u.Ativo
        //     }).ToListAsync();
        //
        //
        // }
        //
        // public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
        // {
        //     return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        // }
        //
        // public async Task UpdateUsuarioAsync(Usuario usuario)
        // {
        //     _context.Usuarios.Update(usuario);
        //     await _context.SaveChangesAsync();
        // }
    }
}