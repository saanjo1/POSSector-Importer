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
            throw new NotImplementedException();
        }

        public Task<bool> Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Category> Update(Guid id, Category entity)
        {
            throw new NotImplementedException();
        }



        Task<Guid> ICategoryDataService.ManageSubcategories(string gender, string collection, string storageId)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == gender).FirstOrDefault();
                Guid season = this.ManageCategories(collection, storageId).Result;

                var category = context.Categories.Where(x => x.Id == season).FirstOrDefault();

                if (subcategory == null)
                {
                    SubCategory subCategory = new SubCategory()
                    {
                        Id = Guid.NewGuid(),
                        Name = gender,
                        Deleted = false,
                        StorageId = category.StorageId,
                        CategoryId = season
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

        public Task<Guid> ManageCategories(string col, string storageId)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                var category = context.Categories.Where(x => x.Name == col).FirstOrDefault();

                if (category == null)
                {
                    Category newCategory = new Category()
                    {
                        Id = Guid.NewGuid(),
                        Name = col,
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
                Storage newStorage = new Storage
                {
                    Id = Guid.NewGuid(),
                };

                switch (storageName)
                {
                    case "1":
                        newStorage.Name = "Articles";
                        break;
                    case "2":
                        newStorage.Name = "Economato";
                        break;
                    default:
                        break;
                }

                var storage = _context.Storages.FirstOrDefault(x => x.Name == newStorage.Name);

                if(storage == null)
                {
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

        
    }

}

