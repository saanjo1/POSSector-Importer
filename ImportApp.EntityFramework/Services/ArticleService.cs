using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Cryptography.X509Certificates;

namespace ImportApp.EntityFramework.Services
{
    public class ArticleService : IArticleService
    {
        public static ImporterDbContextFactory factory;

        public ArticleService(ImporterDbContextFactory _factory)
        {
            factory = _factory;
        }

        public async Task<Guid> CompareArticlesByBarcode(string value)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                var entity = await context.Articles.FirstOrDefaultAsync(x => x.BarCode == value);

                if(entity != null)
                {
                    return entity.Id;
                }   
                else
                {
                    return Guid.Empty;
                }
            }
        }

        public async Task<bool> Create(Article entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                try
                {
                    context.Add(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<ICollection<Article>> Delete(Guid id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Article? entity = await context.Articles.FirstOrDefaultAsync(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChangesAsync();
                ICollection<Article> entities = context.Articles.ToList();
                return entities;
            }
        }

        public async Task<Article> Get(string id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Article? entity = await context.Articles.FirstOrDefaultAsync(x => x.Id.ToString() == id);
                return entity;
            }
        }

        public async Task<ICollection<Article>> GetAll()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Article> entities = await context.Articles.ToListAsync();
                return await Task.FromResult(entities);
            }
        }

        public async Task<Article> Update(Guid id, Article entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Articles.Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }

        public async Task<int> GetArticlesCount()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                int counter = await context.Articles.CountAsync();

                return await Task.FromResult(++counter);
            }

        }

        public async Task<Guid> GetUnitByName(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                var unit = await context.MeasureUnits.FirstOrDefaultAsync(x => x.Name == name);
                if (unit != null)
                    return await Task.FromResult(unit.Id);
                else
                {
                    MeasureUnit newUnit = new MeasureUnit()
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };

                    return newUnit.Id;
                }
            }
        }

        public async Task<Guid> GetGoodId(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Good? good = context.Goods.FirstOrDefault(x => x.Name.Contains(name));

                if(good != null)
                {
                    return good.Id;
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }




        public async Task<decimal> GroupGoodsById(Guid goodId, Guid storageId)
        {
            decimal sumQuantities = 0;

            using (ImporterDbContext context = factory.CreateDbContext())
            {
                sumQuantities = context.InventoryItemBases.
                   Where(x => x.GoodId == goodId && x.StorageId == storageId).Sum(x => x.Quantity);

              InventoryItemBasis? id = context.InventoryItemBases.Where(x => x.GoodId == goodId && x.StorageId == storageId).FirstOrDefault();


            }

            return await Task.FromResult(sumQuantities);
        }

        public async Task<List<Good>> GetGoods()
        {
            List<Good> goods;
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                goods = context.Goods.ToList();
            }

            return await Task.FromResult(goods);
        }

        public Task<decimal> GetTotalSellingPrice(InventoryDocument inventoryDocument)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                List<Guid?> listofGoodIds = context.InventoryItemBases.Where(x => x.InventoryDocumentId == inventoryDocument.Id).Select(x=>x.GoodId).ToList();
                decimal total = 0;

                foreach (var item in listofGoodIds)
                {
                    Guid? articleId = context.ArticleGoods.Where(x => x.GoodId == item).Select(x => x.ArticleId).FirstOrDefault();
                    total += context.Articles.Where(x => x.Id == articleId).Select(x => x.Price).FirstOrDefault();
                }


                return Task.FromResult(Math.Round(total, 2));
            }
        }
    }
}
