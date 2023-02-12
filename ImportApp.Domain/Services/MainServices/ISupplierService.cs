using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface ISupplierService : IGenericBaseInterface<Supplier>
    {
        Task<Guid> GetSupplierByName(string name);
        Task<List<string>> GetListOfSuppliers();
    }
}
