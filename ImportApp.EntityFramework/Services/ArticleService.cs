using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class ArticleService : IArticleService
    {
        public static ImporterDbContextFactory factory;

        public ArticleService(ImporterDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<Guid> CompareArticlesByBarcode(string value)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                var entity = context.Articles.FirstOrDefault(x => x.BarCode == value);

                if(entity != null)
                {
                    return Task.FromResult(entity.Id);
                }
                else
                {
                    return Task.FromResult(Guid.Empty);
                }
            }
        }

        public Task<bool> Create(Article entity)
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

        public Task<ICollection<Article>> Delete(Guid id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
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
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Article? entity = context.Articles.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Article>> GetAll()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Article> entities = context.Articles.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Article> Update(Guid id, Article entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Articles.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }

        public Task<int> GetArticlesCount()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                int counter = context.Articles.Count();

                return Task.FromResult(++counter);
            }

        }

        public Task<Guid> GetUnitByName(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                var unit = context.MeasureUnits.FirstOrDefault(x => x.Name == name);
                if (unit != null)
                    return Task.FromResult(unit.Id);
                else
                {
                    MeasureUnit newUnit = new MeasureUnit()
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };

                    return Task.FromResult(newUnit.Id);
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
    }
}
