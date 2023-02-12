using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using ImportApp.Domain.Services;
using ImportApp.WPF.ViewModels;
using System.Collections.Concurrent;
using ToastNotifications;

namespace ImportApp.WPF.State.Navigators
{
    public partial class Navigator : ObservableObject, INavigator
    {

        [ObservableProperty]
        private BaseViewModel? _currentViewModel;

        [ObservableProperty]
        private string caption;

        [ObservableProperty]
        private IconChar icon;




        private IArticleService _articleService;
        private ICategoryService _categoryService;
        private IStorageService _storeService;
        private IExcelDataService _excelDataService;
        private ConcurrentDictionary<string, string> _myDictionary;
        private IRuleService _discountDataService;
        private ISupplierService _supplierDataService;
        private Notifier _notifier;

        public Navigator(IArticleService articleService, IExcelDataService excelDataService, ICategoryService categoryService, IRuleService discountDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier, IStorageService storeService, ISupplierService supplierDataService)
        {
            _articleService = articleService;
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _storeService = storeService;
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
            _supplierDataService = supplierDataService;
            DefaultLoad();
        }


        [RelayCommand]
        public void EditCurrentViewModel(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService, _supplierDataService);
                        Caption = "Dashboard";
                        Icon = IconChar.Home;
                        break;
                    case ViewType.Discounts:
                        this.CurrentViewModel = new DiscountViewModel(_excelDataService, _notifier, _myDictionary, _articleService, _categoryService, _discountDataService);
                        Caption = "Discounts";
                        Icon = IconChar.Percentage;
                        break;
                    case ViewType.Articles:
                        this.CurrentViewModel = new StoreViewModel(_articleService, _notifier, _storeService, _categoryService);
                        Caption = "Store";
                        Icon = IconChar.TableList;
                        break;
                    case ViewType.ImportArticles:
                        this.CurrentViewModel = new ImportDataViewModel(_excelDataService, _categoryService, _articleService, _notifier, _myDictionary, _storeService, _supplierDataService);
                        Caption = "Import";
                        Icon = IconChar.FileExcel;
                        break;
                    case ViewType.Settings:
                        this.CurrentViewModel = new SettingsViewModel(_discountDataService, _myDictionary, _notifier, _excelDataService);
                        Caption = "Settings";
                        Icon = IconChar.Gear;
                        break;
                    case ViewType.Log:
                        this.CurrentViewModel = new LoginViewModel();
                        Caption = "Login";
                        Icon = IconChar.SignIn;
                        break;
                    default:
                        break;
                }
            }
        }

        public void DefaultLoad()
        {
            this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService, _supplierDataService);
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }




    }
}
