namespace API_AGENDA.Repository.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync(Guid usuarioId);
    Task<T> GetByIdAsync(int id, Guid usuarioId);
    Task AddAsync(T contato);
    Task UpdateAsync(T contato);
    Task DeleteAsync(T contato);
    
}