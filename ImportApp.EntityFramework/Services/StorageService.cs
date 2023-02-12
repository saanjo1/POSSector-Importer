using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;

namespace ImportApp.EntityFramework.Services
{
    public class StorageService : IStorageService
    {
        public static ImporterDbContextFactory factory;

        public StorageService(ImporterDbContextFactory _factory)
        {
            factory = _factory;
        }

        public Task<bool> Create(Storage entity)
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

        public Task<ICollection<Storage>> Delete(Guid id)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
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
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Storage? entity = context.Storages.FirstOrDefault(x => x.Id.ToString() == id);
                return Task.FromResult(entity);
            }
        }

        public Task<ICollection<Storage>> GetAll()
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                ICollection<Storage> entities = context.Storages.ToList();
                return Task.FromResult(entities);
            }
        }

        public async Task<Guid> GetStorageByName(string name)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                Storage _storage = context.Storages.FirstOrDefault(x => x.Name == name);

                if (_storage != null && _storage.Deleted == false)
                    return await Task.FromResult(_storage.Id);
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

                    return await Task.FromResult(newStorage.Id);

                }
            }
        }

        public Task<Storage> Update(Guid id, Storage entity)
        {
            using (ImporterDbContext context = factory.CreateDbContext())
            {
                entity.Id = id;
                context.Storages.Update(entity);
                context.SaveChanges();

                return Task.FromResult(entity);
            }
        }
    }
}
