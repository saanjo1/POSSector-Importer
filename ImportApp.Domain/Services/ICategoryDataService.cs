using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface ICategoryDataService : IDataGService<Category>
    {
        Task<Category> Compare(string value);
        Task<Guid> ManageSubcategories(string value2, string storageId);
        Task<List<string>> GetNamesOfCategories();
        Task<Guid> GetCategoryByName(string name);


        Task<bool> CreateGood(Good good);
        Task<bool> CreateArticleGood(ArticleGood good);
        Task<Guid> GetGoodByName(string good);
        Task<bool> CreateInventoryItem(InventoryItemBasis good);
        Task<bool> CreateInventoryDocument(InventoryDocument good);

        Task<bool> DeleteInventoryDocument(Guid good);

        Task<bool> DeleteInventoryItem(Guid good);

        Task<int> GetInventoryCounter();

        Task<Good> UpdateGood(Guid goodId, Good good);

        Task<bool> CheckArticleGoods(Guid articleId);






    }
}
