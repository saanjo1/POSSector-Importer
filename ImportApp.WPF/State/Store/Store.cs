using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.WPF.ViewModels;
using ToastNotifications;

namespace ImportApp.WPF.State.Store
{
    public partial class Store : ObservableObject, IStore
    {
        [ObservableProperty]
        private BaseViewModel? currentDataGrid;
        private Notifier _notifier;

        private IArticleService _articleService;
        private IStorageService _storageDataService;
        private ICategoryService _categoryDataService;

        public Store(IArticleService articleService, Notifier notifier, IStorageService storageDataService, ICategoryService categoryDataService)
        {
            _articleService = articleService;
            _notifier = notifier;
            _storageDataService = storageDataService;
            _categoryDataService = categoryDataService;
        }

        [RelayCommand]
        public void EditCurrentDataGrid(object? parameter)
        {
            if (parameter is StoreType)
            {
                StoreType storeType = (StoreType)parameter;
                switch (storeType)
                {
                    case StoreType.Articles:
                        this.CurrentDataGrid = new ArticleStorageViewModel(_articleService, "Articles", _notifier, _storageDataService, _categoryDataService);
                        break;
                    case StoreType.Economato:
                        this.CurrentDataGrid = new ArticleStorageViewModel(_articleService, "Economato", _notifier, _storageDataService, _categoryDataService);
                        break;
                    default:
                        break;
                }
            }
        }



    }
}
