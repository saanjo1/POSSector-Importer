using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface ICategoryDataService : IDataGService<Category>
    {
        Task<Category> Compare(string value);
        Task<Guid> ManageSubcategories(string value2, string storageId);
        Task<List<string>> GetNamesOfCategories();
        Task<Guid> GetCategoryByName(string name);
    }
}
