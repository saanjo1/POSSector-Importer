using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IDiscountDataService : IDataGService<Rule>
    {
        Task<List<string>> GetNamesOfDiscounst();
        Task<Guid> GetDiscountByName(string name);
        Task<bool> CreateDiscountItem(RuleItem rule);
    }
}
