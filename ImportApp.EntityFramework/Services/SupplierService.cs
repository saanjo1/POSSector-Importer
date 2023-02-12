using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class SupplierService : ISupplierService
    {
        public static ImporterDbContextFactory factory;

        public SupplierService(ImporterDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<bool> Create(Supplier entity)
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

        public Task<ICollection<Supplier>> Delete(Guid id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Storage? entity = context.Storages.FirstOrDefault(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChanges();
                ICollection<Supplier> entities = context.Suppliers.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Supplier> Get(string id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Supplier? entity = context.Suppliers.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Supplier>> GetAll()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Supplier> entities = context.Suppliers.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Supplier> Update(Guid id, Supplier entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Suppliers.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }


        public Task<Guid> GetSupplierByName(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Supplier _supplier = context.Suppliers.FirstOrDefault(x => x.Name == name);

                if (_supplier != null && _supplier.IsDeleted == false)
                    return Task.FromResult(_supplier.Id);
                else
                {
                    Supplier supplier = new Supplier
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        IsDeleted = false,
                    };

                    context.Suppliers.Add(supplier);
                    context.SaveChanges();

                    return Task.FromResult(supplier.Id);
                }


            }
        }



        public Task<List<string>> GetListOfSuppliers()
        {
            List<string> _suppliersList = new List<string>();

            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Supplier> suppliers = GetAll().Result;

                foreach (var item in suppliers)
                {
                    _suppliersList.Add(item.Name);
                }
            }

            return Task.FromResult(_suppliersList);
        }
    }
}
