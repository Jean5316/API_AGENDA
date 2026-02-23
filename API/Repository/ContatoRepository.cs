using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Repository
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
            return await _context.Contatos.Where(c => c.UsuarioId == usuarioId).ToListAsync();
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

        public async Task<List<Contato>> GetName(string Nome, int usuarioId)
        {
           return await _context.Contatos.Where(c => c.UsuarioId == usuarioId && c.Nome.ToLower().StartsWith(Nome.ToLower())).ToListAsync();

        }

        public async Task<PaginacaoResponse<Contato>> ListaPaginadoAsync(int usuarioId, int pagina, int tamanhoPagina)
        {
            var query = _context.Contatos.Where(c => c.UsuarioId == usuarioId);
            
            var totalRegistros = await query.CountAsync();

            var dados = await query.OrderBy(c => c.Nome)
                                   .Skip((pagina - 1) * tamanhoPagina)
                                   .Take(tamanhoPagina)
                                   .ToListAsync();

            return new PaginacaoResponse<Contato>
            {
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina),
                Dados = dados
            };
        }
    }
}