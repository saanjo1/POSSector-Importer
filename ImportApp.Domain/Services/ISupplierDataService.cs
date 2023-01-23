using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface ISupplierDataService : IDataGService<Supplier>
    {
        Task<Guid> GetSupplierByName(string name);
        Task<List<string>> GetListOfSuppliers();
    }
}
