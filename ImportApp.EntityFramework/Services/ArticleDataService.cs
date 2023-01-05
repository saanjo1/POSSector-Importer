using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics.Metrics;
using System.Reflection;

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

        public Task<bool> ManageArticleGood(ArticleGood article)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                try
                {
                    var articleGood = context.ArticleGoods.Where(x => x.Id == article.Id).FirstOrDefault();

                    if(articleGood == null)
                    {
                        context.ArticleGoods.Add(article);
                    }
                    else
                    {
                        articleGood.Quantity += article.Quantity;
                    }

                    context.SaveChanges();
                    return Task.FromResult(true);

                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }

        }

        public Task<ICollection<Article>> Delete(Guid id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Article? entity = context.Articles.FirstOrDefault(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChanges();
                ICollection<Article> entities = context.Articles.ToList();
                return Task.FromResult(entities);
            }
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

        public Task<ICollection<Article>> GetArticles()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                List<Article> entities = new List<Article>();

                foreach (var item in context.Articles)
                {
                    SubCategory? subCategory = context.SubCategories.Where(x => x.Id == item.SubCategoryId).FirstOrDefault();

                    Storage? storage = context.Storages.Where(x => x.Id == subCategory.StorageId).FirstOrDefault();

                    if(storage?.Name == "Articles" && item.Deleted == false)
                    {
                        entities.Add(item);
                    }
                }

                ICollection<Article> entitiesCollection = entities;
                return Task.FromResult(entitiesCollection);
            }
        }
        public Task<ICollection<Article>> GetEconomato()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                List<Article> entities = new List<Article>();

                foreach (var item in context.Articles)
                {
                    SubCategory? subCategory = context.SubCategories.Where(x => x.Id == item.SubCategoryId).FirstOrDefault();

                    Storage? storage = context.Storages.Where(x => x.Id == subCategory.StorageId).FirstOrDefault();

                    if (storage?.Name == "Economato" && item.Deleted == false)
                    {
                        entities.Add(item);
                    }
                }

                ICollection<Article> entitiesCollection = entities;
                return Task.FromResult(entitiesCollection);
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

        public Task<int> GetLastArticleNumber()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                int counter = context.Articles.Count();

                return Task.FromResult(++counter);
            }

        }

        public Task<ICollection<SubCategory>> GetAllSubcategories()
        {
            using(ImportAppDbContext context = factory.CreateDbContext())
            {
                ICollection<SubCategory> entities = context.SubCategories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Guid> GetSubCategory(string name)
        {
            using(ImportAppDbContext context = factory.CreateDbContext())
            {
                SubCategory? subcategory = context.SubCategories.FirstOrDefault(x=> x.Name == name);

                return Task.FromResult(subcategory.Id);
            }
        }
    }
}
