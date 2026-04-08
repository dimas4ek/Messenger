namespace Application.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task<List<T>> GetAll();
    Task Add(T entity);
    void Remove(T entity);

    Task
        Save(); //todo можно убрать методы Save() из интерфейсов и добавить в IUnitOfWork https://chatgpt.com/s/t_69b6b2e0ca808191b0eebff991c1fc34
}