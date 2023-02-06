using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private IArticleDataService _articleService;
        private IDiscountDataService _discountDataService;
        private IStorageDataService _storeDataService;
        private ICategoryDataService _categoryDataService;
        private ISupplierDataService _supplierDataService;
        private Notifier _notifier;


        public HomeViewModel(IArticleDataService articleService, Notifier notifier, IDiscountDataService discountDataService, IStorageDataService storeDataService, ICategoryDataService categoryDataService, ISupplierDataService supplierDataService)
        {
            _articleService = articleService;
            _notifier = notifier;
            _discountDataService = discountDataService;
            _storeDataService = storeDataService;
            _categoryDataService = categoryDataService;
            _supplierDataService = supplierDataService;
        }

  

    }
}

