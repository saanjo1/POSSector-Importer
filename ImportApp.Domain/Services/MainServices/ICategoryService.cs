using ImportApp.Domain.Models;
using System.Collections.ObjectModel;

namespace ImportApp.Domain.Services
{
    public interface ICategoryService : IGenericBaseInterface<Category>
    {

        Task<Guid> ManageSubcategories(string value2, string storageId);
        Task<List<string>> GetNamesOfCategories();
        Task<Guid> GetCategoryByName(string name);


        Task<bool> CreateGood(Good good);
        Task<bool> CreateArticleGood(ArticleGood good);
        Task<Guid> GetGoodByName(string good);
        Task<bool> CreateInventoryItem(InventoryItemBasis good);
        Task<bool> CreateInventoryDocument(InventoryDocument good);

        Task<ObservableCollection<InventoryDocument>> GetInventoryDocuments();

        Task<decimal?> GetTotalInventoryItems(string _documentId);

        Task<bool> DeleteInventoryDocument(Guid good);

        Task<bool> DeleteInventoryItem(Guid good);

        Task<int> GetInventoryCounter();

        Task<Good> UpdateGood(Guid goodId, Good good);

        Task<bool> CheckArticleGoods(Guid articleId);






    }
}
