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
    public class StoreDataService : IStoreDataService
    {
        public static ImportAppDbContextFactory factory;

        public StoreDataService(ImportAppDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<bool> Create(Storage entity)
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

        public Task<ICollection<Storage>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Storage> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Storage>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Storage> Update(Guid id, Storage entity)
        {
            throw new NotImplementedException();
        }
    }
}
