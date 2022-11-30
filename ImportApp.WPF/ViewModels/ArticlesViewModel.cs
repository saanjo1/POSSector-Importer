using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.EntityFramework.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class ArticlesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ICollection<Article> articleList;

        private string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                textToFilter = value;
                OnPropertyChanged(nameof(TextToFilter));
                ArticleCollection.Filter = FilterFunction;
            }
        }


        [ObservableProperty]
        private ICollectionView articleCollection;

        private IArticleService _articleService;

        [RelayCommand]
        public void LoadData()
        {
            ArticleList = _articleService.GetAll().Result;
            ArticleCollection = CollectionViewSource.GetDefaultView(ArticleList);
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

        public ArticlesViewModel(IArticleService articleService)
        {
            _articleService = articleService;
            LoadData();
        }

    }
}