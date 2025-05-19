using System.Linq.Expressions;
using Domain.Entities;

namespace Infrastructure.Data.Repositories;

public interface IRepository<T> where T : class
{
    // Get methods
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    
    // Add methods
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    // Update methods
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    
    // Delete methods
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
    // Count methods
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    
    // Exists method
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
