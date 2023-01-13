using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface ISubcategoryDataService : IDataGService<SubCategory>
    {
        Task<List<string>> GetNamesOfSubCategories();
        Task<Guid> GetSubCategoryByName(string name);
    }
}
