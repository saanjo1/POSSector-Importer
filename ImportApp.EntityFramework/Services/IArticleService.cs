using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.Services
{
    public class IArticleService : IDataService
    {

        public static ImportAppDbContextFactory factory = new ImportAppDbContextFactory();



        public Task<Article> Create(Article entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Article> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Article>> GetAll()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                ICollection<Article> entities = context.Articles.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Article> Update(Guid id, Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
