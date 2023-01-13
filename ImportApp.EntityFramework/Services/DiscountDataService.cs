using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.Services
{
    public class DiscountDataService : IDiscountDataService
    {

        public static ImportAppDbContextFactory factory;

        public DiscountDataService(ImportAppDbContextFactory _factory) 
        {
           factory = _factory;
        }


        public Task<bool> Create(Rule entity)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
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
            using (ImportAppDbContext context = factory.CreateDbContext())
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
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Rule? entity = context.Rules.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Rule>> GetAll()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                ICollection<Rule> entities = context.Rules.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Guid> GetDiscountByName(string name)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Rule _discount = context.Rules.FirstOrDefault(x => x.Name == name);

                if (_discount != null)
                    return Task.FromResult(_discount.Id);


                return Task.FromResult(Guid.Empty);
            }
        }

        public Task<List<string>> GetNamesOfDiscounst()
        {
            ICollection<Rule> discountCollection = GetAll().Result;
            List<string> discounts = new List<string>();

            if (discountCollection.Count > 0)
            {
                foreach (Rule item in discountCollection)
                {
                    discounts.Add(item.Name);
                }
            }

            return Task.FromResult(discounts);
        }

        public Task<Rule> Update(Guid id, Rule entity)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Rules.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }


        public Task<bool> CreateDiscountItem(RuleItem entity)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
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
