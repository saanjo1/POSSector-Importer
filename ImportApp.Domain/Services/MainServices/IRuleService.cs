using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface IRuleService : IGenericBaseInterface<Rule>
    {
        Task<Rule> GetRuleByName(string _ruleName);
        Task<bool> CreateRuleItem(RuleItem _ruleItem);
    }
}
