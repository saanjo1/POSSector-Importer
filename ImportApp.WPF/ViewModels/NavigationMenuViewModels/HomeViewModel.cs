using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private IArticleService _articleService;
        private IRuleService _discountDataService;
        private IStorageService _storeDataService;
        private ICategoryService _categoryDataService;
        private ISupplierService _supplierDataService;
        private Notifier _notifier;


        public HomeViewModel(IArticleService articleService, Notifier notifier, IRuleService discountDataService, IStorageService storeDataService, ICategoryService categoryDataService, ISupplierService supplierDataService)
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

