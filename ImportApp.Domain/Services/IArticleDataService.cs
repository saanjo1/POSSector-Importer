﻿using ImportApp.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IArticleDataService : IDataGService<Article>
    {

        Task<Article> Compare(string value);
        Task<int> GetLastArticleNumber();
        Task<ICollection<Article>> GetArticles();
        Task<ICollection<Article>> GetEconomato();
        Task<bool> ManageArticleGood(ArticleGood article);

        Task<Guid> GetUnitByName(string name);
        Task<Guid> GetGoodId(string name);
        Task<List<Good>> GetGoods();
        Task<decimal> GroupGoodsById(Guid goodId, Guid storageId);


        Task<ICollection<SubCategory>> GetAllSubcategories();
        Task<Guid> GetSubCategory(string name);

        Task<Guid> GetArticleByName(string name);

    }
}
