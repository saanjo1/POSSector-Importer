using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToastNotifications;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ImportApp.WPF.State.Store
{
    public partial class Store : ObservableObject, IStore
    {
        [ObservableProperty]
        private BaseViewModel? currentDataGrid;
        private Notifier _notifier;

        private IArticleDataService _articleService;
        private IStorageDataService _storageDataService;
        private ICategoryDataService _categoryDataService;

        public Store(IArticleDataService articleService, Notifier notifier, IStorageDataService storageDataService, ICategoryDataService categoryDataService)
        {
            _articleService = articleService;
            _notifier = notifier;
            _storageDataService = storageDataService;
            _categoryDataService = categoryDataService;
        }

        [RelayCommand]
        public void EditCurrentDataGrid(object? parameter)
        {
            if(parameter is StoreType)
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
