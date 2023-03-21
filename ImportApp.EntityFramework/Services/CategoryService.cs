using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using System.Collections.ObjectModel;

namespace ImportApp.EntityFramework.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ImporterDbContextFactory _contextFactory;

        public CategoryService(ImporterDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task<bool> Create(Category entity)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                Category? entity = context.Categories.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Category>> GetAll()
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                ICollection<Category> entities = context.Categories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Category> Update(Guid id, Category entity)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
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


        Task<Guid> ICategoryService.ManageSubcategories(string ctgry, string storageId)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == ctgry).FirstOrDefault();
                Guid ctgryId = this.ManageCategories(ctgry, storageId).Result;

                var category = context.Categories.Where(x => x.Id == ctgryId).FirstOrDefault();

                if (subcategory == null)
                {
                    SubCategory subCategory = new SubCategory()
                    {
                        Id = Guid.NewGuid(),
                        Name = ctgry,
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
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
            using (ImporterDbContext _context = _contextFactory.CreateDbContext())
            {

                var storage = _context.Storages.FirstOrDefault(x => x.Name == storageName);

                if (storage == null)
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                var _category = context.Categories.FirstOrDefault(x => x.Name == name);

                if (_category != null && _category.Deleted == false)
                    return Task.FromResult(_category.Id);

                return null;
            }
        }

        public Task<bool> CreateGood(Good good)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Goods.Add(good);
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.ArticleGoods.Add(good);
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.InventoryItemBases.Add(good);
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.InventoryDocuments.Add(good);
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
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                Good _good = context.Goods.FirstOrDefault(x => x.Name == good);

                if (_good != null)
                    return Task.FromResult(_good.Id);
                else
                    return Task.FromResult(Guid.Empty);
            }
        }

        public Task<Good> UpdateGood(Guid id, Good entity)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;
                context.Goods.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }


        public Task<int> GetInventoryCounter()
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                int counter = context.InventoryDocuments.Count();

                return Task.FromResult(counter++);
            }
        }

        public Task<bool> DeleteInventoryDocument(Guid docId)
        {

            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                var doc = context.InventoryDocuments.FirstOrDefault(x => x.Id == docId);
                if (doc != null)
                {
                    context.InventoryDocuments.Remove(doc);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteInventoryItem(Guid docId)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                var doc = context.InventoryItemBases.FirstOrDefault(x => x.Id == docId);
                if (doc != null)
                {
                    context.InventoryItemBases.Remove(doc);
                    return await Task.FromResult(true);
                }

                return await Task.FromResult(false);
            }
        }


        public async Task<bool> CheckArticleGoods(Guid docId)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                var doc = context.ArticleGoods.FirstOrDefault(x => x.ArticleId == docId);
                if (doc != null)
                {
                    return await Task.FromResult(true);
                }

                return await Task.FromResult(false);
            }
        }

        public Task<ObservableCollection<InventoryDocument>> GetInventoryDocuments()
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                ObservableCollection<InventoryDocument> inventoryDocuments = new ObservableCollection<InventoryDocument>(); 
                var listOfInventoryDocuments = context.InventoryDocuments.OrderByDescending(x=>x.Created).Where(x => x.IsDeleted == false).ToList();
                foreach (var inventoryDocument in listOfInventoryDocuments)
                {
                    inventoryDocuments.Add(inventoryDocument);
                }

                return Task.FromResult(inventoryDocuments);

            }
        }

        public Task<decimal> GetTotalInventoryItems(string _documentId)
        {
            using (ImporterDbContext context = _contextFactory.CreateDbContext())
            {
                decimal? total = context.InventoryItemBases.Where(x => x.InventoryDocumentId.ToString() == _documentId).Sum(x => x.Total);

                return Task.FromResult(Math.Round((decimal)total, 2));
            }
        }
    }

}

