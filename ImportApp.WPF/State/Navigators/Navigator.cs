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


        private IArticleDataService _articleService;
        private ICategoryDataService _categoryService;
        private IStoreDataService _storeService;
        private IExcelDataService _excelDataService;
        private ConcurrentDictionary<string, string> _myDictionary;
        private IDiscountDataService _discountDataService;
        private Notifier _notifier;

        public Navigator(IArticleDataService articleService, IExcelDataService excelDataService, ICategoryDataService categoryService, IDiscountDataService discountDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier, IStoreDataService storeService)
        {
            _articleService = articleService;
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _storeService = storeService;
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
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
                        this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService);
                        Caption = "Dashboard";
                        Icon = IconChar.Home;
                        break;
                    case ViewType.Articles:
                        this.CurrentViewModel = new StoreViewModel(_articleService, _notifier);
                        Caption = "Store";
                        Icon = IconChar.TableList;
                        break;
                    case ViewType.ImportArticles:
                        this.CurrentViewModel = new ImportDataViewModel(_excelDataService, _categoryService, _articleService, _notifier, _myDictionary);
                        Caption = "Import";
                        Icon = IconChar.FileExcel;
                        break;
                    case ViewType.Settings:
                        this.CurrentViewModel = new SettingsViewModel(_discountDataService, _myDictionary, _notifier);
                        Caption = "Settings";
                        Icon = IconChar.Gear;
                        break;
                    default:
                        break;
                }
            }
        }

        public void DefaultLoad()
        {
            this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService);
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }


    }
}
