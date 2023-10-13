using Backend_API.Entities;
using Core.Specifications;

namespace Core.Interfaces;

public interface IGenericRepository<T>
{
    public Task<T?> GetByIdAsync(int id);
    
    public Task<IReadOnlyList<T>> GetAllAsync();

    public Task<T?> GetEntityWithSpecification(ISpecification<T> specification);

    public Task<IReadOnlyList<T>> ListAllAsync(ISpecification<T> specification);

    public Task<int> CountAsync(ISpecification<T> specification);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);
}