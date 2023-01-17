using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface ISupplierDataService : IDataGService<Supplier>
    {
        Task<Guid> GetSupplierByName (string name);
        Task<List<string>> GetListOfSuppliers();
    }
}
