using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface ISubcategoryDataService : IDataGService<SubCategory>
    {
        Task<List<string>> GetNamesOfSubCategories();
        Task<Guid> GetSubCategoryByName(string name);
    }
}
