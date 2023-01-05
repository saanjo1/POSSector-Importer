using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private IArticleDataService _articleService;
        private IDiscountDataService _discountDataService;
        private IStoreDataService _storeDataService;
        private ICategoryDataService _categoryDataService;
        private Notifier _notifier;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddArticleCommand))]
        private bool isOpen;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddRuleCommand))]
        private bool isDiscountOpen;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddStoreCommand))]
        private bool isStoreOpen;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCategoryCommand))]
        private bool isCategoryOpen;

        [ObservableProperty]
        private CreateNewArticleViewModel addArticleModel;
        
        [ObservableProperty]
        private CreateNewDiscountViewModel addRuleViewModel;

        [ObservableProperty]
        private CreateNewStoreViewModel addStoreViewModel;
        
        [ObservableProperty]
        private CreateCategoryViewModel addCategoryViewModel;



        public HomeViewModel(IArticleDataService articleService, Notifier notifier, IDiscountDataService discountDataService, IStoreDataService storeDataService, ICategoryDataService categoryDataService)
        {
            _articleService = articleService;
            _notifier = notifier;
            _discountDataService = discountDataService;
            _storeDataService = storeDataService;
            _categoryDataService = categoryDataService;
        }

        [RelayCommand]
        private void AddArticle()
        {
            IsOpen = true;
            AddArticleModel = new CreateNewArticleViewModel(_articleService, this, _notifier);
        }

        [RelayCommand]
        private void AddRule()
        {
            IsDiscountOpen = true;
            AddRuleViewModel = new CreateNewDiscountViewModel(this, _discountDataService, _notifier);
        }

        [RelayCommand]
        private void AddStore()
        {
            IsStoreOpen = true;
            AddStoreViewModel = new CreateNewStoreViewModel(this, _notifier, _storeDataService);

        }

        [RelayCommand]
        private void AddCategory()
        {
            IsCategoryOpen = true;
            AddCategoryViewModel = new CreateCategoryViewModel(this, _notifier, _categoryDataService);

        }

        [RelayCommand]
        public void Close()
        {
            if (IsOpen)
                IsOpen = false;
            else if (IsDiscountOpen)
                IsDiscountOpen = false;
            else if (IsStoreOpen)
                IsStoreOpen = false;
            else if (IsCategoryOpen)
                IsCategoryOpen = false;
        }

    }
}

