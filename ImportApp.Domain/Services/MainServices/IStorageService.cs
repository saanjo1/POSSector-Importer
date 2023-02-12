using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface IStorageService : IGenericBaseInterface<Storage>
    {
        Task<Guid> GetStorageByName(string name);
    }
}
