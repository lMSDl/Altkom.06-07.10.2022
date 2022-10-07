using Models;

namespace Services.Interfaces
{
    public interface ICrudService<T> where T : Entity
    {
        Task<int> CreateAsync(T entity);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAsync();
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}