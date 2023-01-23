using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class ArticleStorageViewModel : BaseViewModel
    {
        private IArticleDataService _articleService;
        private IStorageDataService _storageService;
        private ICategoryDataService _categoryDataService;
        private Notifier _notifier;

        [ObservableProperty]
        private string storageName;

        [ObservableProperty]
        private int count;

        [ObservableProperty]
        private bool isEditOpen;

        [ObservableProperty]
        private bool isDeleteOpen;

        [ObservableProperty]
        private EditStorageViewModel editArticleViewModel;


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

        private bool FilterFunction(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var filt = obj as GoodsArticlesViewModel;
                return filt != null && (filt.Name.Contains(TextToFilter));
            }
            return true;
        }



        public ArticleStorageViewModel(IArticleDataService articleService, string _storageName, Notifier notifier, IStorageDataService storageService, ICategoryDataService categoryDataService)
        {
            _notifier = notifier;
            _articleService = articleService;
            storageName = _storageName;
            _storageService = storageService;
            LoadData();
            _categoryDataService = categoryDataService;
        }


        [ObservableProperty]
        private ICollection<GoodsArticlesViewModel> articleList;

        [ObservableProperty]
        private ObservableCollection<GoodsArticlesViewModel> articlesCollection = new ObservableCollection<GoodsArticlesViewModel>();

        [ObservableProperty]
        private ICollectionView articleCollection;




        [RelayCommand]
        public void EditArticle(GoodsArticlesViewModel parameter)
        {
            IsEditOpen = true;
            this.EditArticleViewModel = new EditStorageViewModel(parameter, _notifier, _categoryDataService, _storageService, this);
        }

        [RelayCommand]
        public void LoadData()
        {
            if (StorageName == "Articles")
            {
                ArticleList = StorageQuantityCounter("Articles").Result;
            }
            else
            {
                ArticleList = StorageQuantityCounter("Economato").Result;
            }
            ArticleCollection = CollectionViewSource.GetDefaultView(ArticleList);
            Count = ArticleList.Count;
        }

        [RelayCommand]
        public void Cancel()
        {
            if (IsEditOpen)
                IsEditOpen = false;
            if (IsDeleteOpen)
                IsDeleteOpen = false;
        }


        public Task<ICollection<GoodsArticlesViewModel>> StorageQuantityCounter(string storageName)
        {
            List<Good> goods = _articleService.GetGoods().Result;
            Guid storage = _storageService.GetStorageByName(storageName).Result;
            ICollection<GoodsArticlesViewModel> tempList = new List<GoodsArticlesViewModel>();
            foreach (var item in goods)
            {
                decimal quantity = _articleService.GroupGoodsById(item.Id, storage).Result;
                if (quantity > 0 && quantity != null)
                {
                    tempList.Add(new GoodsArticlesViewModel
                    {
                        Id = Guid.NewGuid(),
                        Name = item.Name,
                        GoodId = _articleService.GetGoodId(item.Name).Result,
                        Quantity = quantity,
                        Storage = storage,
                        Price = Math.Round((item.LatestPrice * quantity), 2),
                        LatestPrice = item.LatestPrice
                    });

                }
            }

            return Task.FromResult(tempList);
        }


    }
}

