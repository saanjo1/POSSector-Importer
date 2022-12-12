using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.Services
{
    public class GenericDataService<T> : IGenericDataService<T> where T : BaseModel
    {
        private readonly ImportAppDbContextFactory _contextFactory;

        public GenericDataService(ImportAppDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(T entity)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Add(entity);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<T> Get(string id)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id.ToString() == id);
                return entity;
            }
        }

        public async Task<ICollection<T>> GetAll()
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                var entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public async Task<T> Update(Guid id, T entity)
        {
            using (ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
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
            using(ImportAppDbContext context = _contextFactory.CreateDbContext())
            {
                SubCategory subcategory = context.SubCategories.Where(x => x.Name == gender).FirstOrDefault();
                var season = ManageCategories(collection);

                if(subcategory == null)
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

    }
}
