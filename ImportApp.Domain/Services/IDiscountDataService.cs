using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface IDiscountDataService : IDataGService<Rule>
    {
        Task<List<string>> GetNamesOfDiscounst();
        Task<Rule> GetDiscountByName(string name);
        Task<bool> CreateDiscountItem(RuleItem rule);
    }
}
