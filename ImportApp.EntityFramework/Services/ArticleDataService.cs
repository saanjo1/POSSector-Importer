using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore.Internal;

namespace ImportApp.EntityFramework.Services
{
    public class ArticleDataService : IArticleDataService
    {
        public static ImportAppDbContextFactory factory;

        public ArticleDataService(ImportAppDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<Article> Compare(string value)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                var entity = context.Articles.FirstOrDefault(x => x.BarCode == value);

                return Task.FromResult(entity);
            }
        }

        public Task<bool> Create(Article entity)
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

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Article> Get(string id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Article? entity = context.Articles.FirstOrDefault(x => x.BarCode == id);
                return Task.FromResult(entity);
            }
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
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Articles.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }

    }
}
