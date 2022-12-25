using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.State.Navigators;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportApp.WPF.State.Store
{
    public partial class Store : ObservableObject, IStore
    {
        [ObservableProperty]
        private BaseViewModel? currentDataGrid;

        private IArticleDataService _articleService;

        public Store(IArticleDataService articleService)
        {
            _articleService = articleService;   
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
                        this.CurrentDataGrid = new ArticleStorageViewModel(_articleService, "Articles");
                        break;
                    case StoreType.Economato:
                        this.CurrentDataGrid = new ArticleStorageViewModel(_articleService, "Economato");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
