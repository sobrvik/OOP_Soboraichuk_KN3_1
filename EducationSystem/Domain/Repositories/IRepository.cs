namespace Domain.Repositories;

public interface IRepository<T>
{
    void Add(T item);
    IReadOnlyList<T> GetAll();
    T? Find(Func<T, bool> predicate);
}
