using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class StorageDataService : IStorageDataService
    {
        public static ImportAppDbContextFactory factory;

        public StorageDataService(ImportAppDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<bool> Create(Storage entity)
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

        public Task<ICollection<Storage>> Delete(Guid id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Storage? entity = context.Storages.FirstOrDefault(x => x.Id == id);
                entity.Deleted = true;

                context.SaveChanges();
                ICollection<Storage> entities = context.Storages.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<Storage> Get(string id)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Storage? entity = context.Storages.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Storage>> GetAll()
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                ICollection<Storage> entities = context.Storages.ToList();
                return Task.FromResult(entities);
            }
        }

        public Task<List<string>> GetNamesOfStorages()
        {
            ICollection<Storage> storageCollection = GetAll().Result;
            List<string> storages = new List<string>();

            if (storageCollection.Count > 0)
            {
                foreach (Storage item in storageCollection)
                {
                    storages.Add(item.Name);
                }
            }

            return Task.FromResult(storages);
        }

        public Task<Guid> GetStorageByName(string name)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                Storage _storage = context.Storages.FirstOrDefault(x => x.Name == name);

                if (_storage != null && _storage.Deleted == false)
                    return Task.FromResult(_storage.Id);
                else
                {
                    Domain.Models.Storage newStorage = new Domain.Models.Storage()
                    {
                        Id = Guid.NewGuid(),
                        Deleted = false,
                        Name = name
                    };

                    context.Storages.Add(newStorage);
                    context.SaveChanges();

                    return Task.FromResult(newStorage.Id);

                }
            }
        }

        public Task<Storage> Update(Guid id, Storage entity)
        {
            using (ImportAppDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Storages.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }
    }
}
