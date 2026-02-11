using API_AGENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API_AGENDA.Context
{
    public class AgendaContext : DbContext
    {
        public AgendaContext(DbContextOptions<AgendaContext> options) : base(options)
        {
        }

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
    
}