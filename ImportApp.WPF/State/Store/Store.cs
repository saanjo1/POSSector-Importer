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

        private string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                textToFilter = value;
                OnPropertyChanged(nameof(TextToFilter));
                //ArticleCollection.Filter = FilterFunction;
            }
        }



        private bool FilterFunction(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var filt = obj as Article;
                return filt != null && (filt.Name.Contains(TextToFilter) || filt.BarCode.Contains(TextToFilter) || filt.Price.ToString() == TextToFilter || filt.ArticleNumber.ToString() == TextToFilter);
            }
            return true;
        }


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
                        this.CurrentDataGrid = new ArticleStorageViewModel(_articleService);
                        break;
                    case StoreType.Economato:
                        this.CurrentDataGrid = new EconomatoStorageViewModel();
                       break;
                    default:
                        break;
                }
            }
        }
    }
}
