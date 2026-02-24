using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using API.Repository.Interfaces;
using API_AGENDA.Context;
using API_AGENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AgendaContext _context;
        public UsuarioRepository(AgendaContext context)
        {
            _context = context;
        }

        public async Task DeleteUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> GetAllUsuariosAsync()
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

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}