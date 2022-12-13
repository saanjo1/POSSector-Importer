using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Guid ManageCategories(string col)
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
                        StorageId = null
                    };
                    var id = newCategory.Id;
                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                    return id;
                }
                else
                {
                    return category.Id;
                }
            }

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
                        CategoryId = season
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

        Task<Guid> ICategoryDataService.ManageSubcategories(string value1, string value2)
        {
            throw new NotImplementedException();
        }

        Task<Guid> ICategoryDataService.ManageCategories(string value1)
        {
            throw new NotImplementedException();
        }
    }
}
