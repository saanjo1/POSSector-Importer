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
        Task<Guid> ManageSubcategories(string value1, string value2);
        Task<Guid> ManageCategories(string value1);
    }
}
