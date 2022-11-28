using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IDataService
    {
        Task<ICollection<Article>> GetAll();

        Task<Article> Get(Guid id);

        Task<Article> Create(Article entity);

        Task<Article> Update(Guid id, Article entity);

        Task<bool> Delete(Guid id);

    }
}
