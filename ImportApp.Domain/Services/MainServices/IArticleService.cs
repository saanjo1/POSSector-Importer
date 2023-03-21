using ImportApp.Domain.Models;

namespace ImportApp.Domain.Services
{
    public interface IArticleService : IGenericBaseInterface<Article>
    {

        Task<Guid> CompareArticlesByBarcode(string _barcode);
        Task<int> GetArticlesCount();
        Task<Guid> GetUnitByName(string name);
        Task<Guid> GetGoodId(string name);
        Task<List<Good>> GetGoods();
        Task<decimal> GroupGoodsById(Guid goodId, Guid storageId);
        Task<decimal> GetTotalSellingPrice(InventoryDocument inventoryDocument);
    }
}
