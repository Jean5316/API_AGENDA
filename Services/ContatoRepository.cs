using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Controllers
{
    public class ContatoRepository : IContatoRepository
    {
        
        private readonly AgendaContext _context;

        public ContatoRepository(AgendaContext context)
        {
            _context = context;
        }

        public async Task<List<Contato>> GetAllContatosAsync(int usuarioId)
        {
            return await _context.Contatos.Where(c => c.Id == usuarioId).ToListAsync();
        }
        public async Task<Contato?> GetContatoByIdAsync(int id, int usuarioId)
        {
            return await _context.Contatos.FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);
        }

        public async Task<List<Contato>> GetFavoritosAsync(int usuarioId)
        {
            return await _context.Contatos.Where(c => c.Ativo && c.Favorito && c.UsuarioId == usuarioId).ToListAsync();
        }


        public async Task AddContatoAsync(Contato contato)
        {
            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateContatoAsync(Contato contato)
        {
            _context.Contatos.Update(contato);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteContatoAsync(Contato contato)
        {

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();

        }
    }
}