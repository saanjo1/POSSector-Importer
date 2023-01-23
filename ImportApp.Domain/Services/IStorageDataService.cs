using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface IStorageDataService : IDataGService<Storage>
    {
        Task<List<string>> GetNamesOfStorages();
        Task<Guid> GetStorageByName(string name);
    }
}
