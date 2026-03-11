using API_AGENDA.Context;
using API_AGENDA.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AgendaContext _context;

    public Repository(AgendaContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<T>> GetAllAsync(Guid usuarioId)
    {
        return await _context.Set<T>()
            .Where(p => EF.Property<Guid>(p, "UsuarioId") == usuarioId).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id, Guid usuarioId)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(c =>
            EF.Property<int>(c, "Id") == id && EF.Property<Guid>(c, "UsuarioId") == usuarioId);
    }

    public async Task AddAsync(T contato)
    {
        _context.Set<T>().Add(contato);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T contato)
    {
        _context.Set<T>().Update(contato);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T contato)
    {
        _context.Set<T>().Remove(contato);
        _context.SaveChanges();
    }
}