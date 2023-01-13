using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IStorageDataService : IDataGService<Storage>
    {
        Task<List<string>> GetNamesOfStorages();
        Task<Guid> GetStorageByName(string name);
    }
}
