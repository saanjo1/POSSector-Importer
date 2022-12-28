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
            throw new NotImplementedException();
        }

        public Task<Rule> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Rule>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Rule> Update(Guid id, Rule entity)
        {
            throw new NotImplementedException();
        }
    }
}
