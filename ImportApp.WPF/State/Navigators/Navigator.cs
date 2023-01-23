using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using ImportApp.Domain.Services;
using ImportApp.WPF.ViewModels;
using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Concurrent;
using System.Windows.Threading;
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
        private IStorageDataService _storeService;
        private IExcelDataService _excelDataService;
        private ISubcategoryDataService _subCategoryDataService;
        private ConcurrentDictionary<string, string> _myDictionary;
        private IDiscountDataService _discountDataService;
        private ISupplierDataService _supplierDataService;
        private Notifier _notifier;

        public Navigator(IArticleDataService articleService, IExcelDataService excelDataService, ICategoryDataService categoryService, IDiscountDataService discountDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier, IStorageDataService storeService, ISubcategoryDataService subCategoryDataService, ISupplierDataService supplierDataService)
        {
            _articleService = articleService;
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _storeService = storeService;
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
            _subCategoryDataService = subCategoryDataService;
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
                        this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService, _subCategoryDataService, _supplierDataService);
                        Caption = "Dashboard";
                        Icon = IconChar.Home;
                        break;
                    case ViewType.Discounts:
                        this.CurrentViewModel = new DiscountViewModel(_excelDataService, _notifier, _myDictionary,_articleService, _categoryService, _discountDataService);
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
                    default:
                        break;
                }
            }
        }

        public void DefaultLoad()
        {
            this.CurrentViewModel = new HomeViewModel(_articleService, _notifier, _discountDataService, _storeService, _categoryService, _subCategoryDataService, _supplierDataService);
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }




    }
}
