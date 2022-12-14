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



        public Guid ManageSubcategories(string gender, string collection)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == gender).FirstOrDefault();
                var season = ManageCategories(collection);

                if (subcategory == null)
                {
                    SubCategory subCategory = new SubCategory()
                    {
                        Id = Guid.NewGuid(),
                        Name = gender,
                        Deleted = false,
                        CategoryId = season.Result
                    };

                    var Id = subCategory.Id;
                    context.Add(subCategory);
                    context.SaveChanges();
                    return Id;
                }
                else
                {
                    return subcategory.Id;
                }
            }
        }

        Task<Guid> ICategoryDataService.ManageSubcategories(string gender, string collection)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == gender).FirstOrDefault();
                var season = this.ManageCategories(collection).Result;

                if (subcategory == null)
                {
                    SubCategory subCategory = new SubCategory()
                    {
                        Id = Guid.NewGuid(),
                        Name = gender,
                        Deleted = false,
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

        public Task<Guid> ManageCategories(string col)
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
                        StorageId = this.ManageStorages(col).Result
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

                if (storage != null)
                {
                    return Task.FromResult(storage.Id);
                }
                else
                {
                    Storage newStorage = new Storage()
                    {
                        Id = Guid.NewGuid(),
                        Name = storageName
                    };

                    _context.Storages.Add(newStorage);
                    _context.SaveChanges();

                    return Task.FromResult(newStorage.Id);
                }
            }
        }
    }
}

