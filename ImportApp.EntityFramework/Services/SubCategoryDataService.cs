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
    public class SubCategoryDataService : ISubcategoryDataService
    {
        public static ImportAppDbContextFactory factory;

        public SubCategoryDataService(ImportAppDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<bool> Create(SubCategory entity)
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

        public Task<ICollection<SubCategory>> Delete(Guid id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                SubCategory? entity = context.SubCategories.FirstOrDefault(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChanges();
                ICollection<SubCategory> entities = context.SubCategories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<SubCategory> Get(string id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                SubCategory? entity = context.SubCategories.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<SubCategory>> GetAll()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                ICollection<SubCategory> entities = context.SubCategories.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<List<string>> GetNamesOfSubCategories()
        {
            ICollection<SubCategory> subCategoryCollection = GetAll().Result;
            List<string> subCategories = new List<string>();

            if (subCategoryCollection.Count > 0)
            {
                foreach (SubCategory item in subCategoryCollection)
                {
                    subCategories.Add(item.Name);
                }
            }

            return Task.FromResult(subCategories);
        }

        public Task<Guid> GetSubCategoryByName(string name)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                var _subCategory = context.SubCategories.FirstOrDefault(x => x.Name == name);

                if (_subCategory != null && _subCategory.Deleted == false)
                    return Task.FromResult(_subCategory.Id);

                return null;
            }
        }

        public Task<SubCategory> Update(Guid id, SubCategory entity)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.SubCategories.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }
    }
}
