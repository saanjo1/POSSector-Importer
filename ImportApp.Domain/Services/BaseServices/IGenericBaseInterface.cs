namespace ImportApp.Domain.Services
{
    public interface IGenericBaseInterface<T>
    {
        Task<ICollection<T>> GetAll();

        Task<T> Get(string id);

        Task<bool> Create(T entity);

        Task<T> Update(Guid id, T entity);

        Task<ICollection<T>> Delete(Guid id);
    }
}
