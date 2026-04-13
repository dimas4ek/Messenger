namespace Application.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task<List<T>> GetAll();
    Task Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task Save();
}