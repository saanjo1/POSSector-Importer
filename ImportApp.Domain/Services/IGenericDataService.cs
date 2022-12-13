using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IDataGService<T> 
    {
        Task<ICollection<T>> GetAll();

        Task<T> Get(string id);

        Task<bool> Create(T entity);

        Task<T> Update(Guid id, T entity);

        Task<bool> Delete(Guid id);
    }
}
