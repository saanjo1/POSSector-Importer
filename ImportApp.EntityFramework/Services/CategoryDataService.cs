using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class CategoryDataService : ICategoryDataService
    {
        private readonly ImportAppDbContextFactory _contextFactory;

        public CategoryDataService(ImportAppDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public Task<Category> Compare(string value)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                var entity = context.Categories.FirstOrDefault(x => x.Id.ToString() == value);

                return Task.FromResult(entity);
            }
        }

        public Task<bool> Create(Category entity)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
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

        public Task<ICollection<Category>> Delete(Guid id)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                Category? entity = context.Categories.FirstOrDefault(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChanges();
                ICollection<Category> entities = context.Categories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Category> Get(string id)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                Category? entity = context.Categories.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Category>> GetAll()
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                ICollection<Category> entities = context.Categories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Category> Update(Guid id, Category entity)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;
                context.Categories.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }

        public Task<List<string>> GetNamesOfCategories()
        {
            ICollection<Category> categoryCollection = GetAll().Result;
            List<string> categories = new List<string>();

            if (categoryCollection.Count > 0)
            {
                foreach (Category item in categoryCollection)
                {
                    categories.Add(item.Name);
                }
            }

            return Task.FromResult(categories);
        }


        Task<Guid> ICategoryDataService.ManageSubcategories(string ctgry, string storageId)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == "DefaultSubCategory").FirstOrDefault();
                Guid ctgryId = this.ManageCategories(ctgry, storageId).Result;

                var category = context.Categories.Where(x => x.Id == ctgryId).FirstOrDefault();

                if (subcategory == null)
                {
                    SubCategory subCategory = new SubCategory()
                    {
                        Id = Guid.NewGuid(),
                        Name = "DefaultSubCategory",
                        Deleted = false,
                        StorageId = category.StorageId,
                        CategoryId = ctgryId
                    };

                    var Id = subCategory.Id;
                    context.Add(subCategory);
                    context.SaveChanges();
                    return Task.FromResult(Id);
                }
                else
                {
                    return Task.FromResult(subcategory.Id);
                }
            }
        }

        public Task<Guid> ManageCategories(string ctgry, string storageId)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                var category = context.Categories.Where(x => x.Name == ctgry).FirstOrDefault();

                if (category == null)
                {
                    Category newCategory = new Category()
                    {
                        Id = Guid.NewGuid(),
                        Name = ctgry,
                        Deleted = false,
                        Order = 1,
                        StorageId = this.ManageStorages(storageId).Result
                    };
                    var id = newCategory.Id;
                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                    return Task.FromResult(id);
                }
                else
                {
                    return Task.FromResult(category.Id);
                }
            }
        }


        public Task<Guid> ManageStorages(string storageName)
        {
            using (ImportAppDbContext _context = _contextFactory.CreateDbContext())
            {
               
                var storage = _context.Storages.FirstOrDefault(x => x.Name == storageName);

                if(storage == null)
                { 
                    Storage newStorage = new Storage
                    {
                        Id = Guid.NewGuid(),
                        Name = storageName, 
                        Deleted = false,
                    };

                    _context.Storages.Add(newStorage);
                    _context.SaveChanges();
                    return Task.FromResult(newStorage.Id);
                }
                else
                {
                    return Task.FromResult(storage.Id);
                }

            }
        }

        public Task<Guid> GetCategoryByName(string name)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                var _category = context.Categories.FirstOrDefault(x => x.Name == name);

                if(_category != null &&_category.Deleted == false)
                    return Task.FromResult(_category.Id);

                return null;
            }
        }

        public Task<bool> CreateGood(Good good)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Add(good);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> CreateArticleGood(ArticleGood good)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Add(good);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> CreateInventoryItem(InventoryItemBasis good)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Add(good);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> CreateInventoryDocument(InventoryDocument good)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Add(good);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
        }

        public Task<Guid> GetGoodByName(string good)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                Good _good = context.Goods.FirstOrDefault(x => x.Name == good);

                if (_good != null)
                    return Task.FromResult(_good.Id);
                else
                    return Task.FromResult(Guid.Empty);
            }
        }
    }

}

