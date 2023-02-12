using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class RuleService : IRuleService
    {

        public static ImporterDbContextFactory factory;

        public RuleService(ImporterDbContextFactory _factory)
        {
            factory = _factory;
        }


        public Task<bool> Create(Rule entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                try
                {
                    context.Add(entity);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<ICollection<Rule>> Delete(Guid id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Rule? entity = context.Rules.FirstOrDefault(x => x.Id == id);
                entity.IsExecuted = true;

                context.SaveChanges();
                ICollection<Rule> entities = context.Rules.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Rule> Get(string id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Rule? entity = context.Rules.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Rule>> GetAll()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Rule> entities = context.Rules.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Rule> GetRuleByName(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Rule _discount = context.Rules.FirstOrDefault(x => x.Name == name);
                return Task.FromResult(_discount);
            }
        }

        public Task<Rule> Update(Guid id, Rule entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Rules.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }


        public Task<bool> CreateRuleItem(RuleItem entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                try
                {
                    context.Add(entity);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }
    }
}
