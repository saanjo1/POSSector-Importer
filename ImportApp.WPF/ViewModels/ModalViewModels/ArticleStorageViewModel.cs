using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client.Extensions.Msal;
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
        private bool isEnabled;

        [ObservableProperty]
        private EditStorageViewModel editArticleViewModel;

        [ObservableProperty]
        private List<InventoryItemBasis> listOfItems = new List<InventoryItemBasis>();



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
            IsEnabled = false;
        }


        [ObservableProperty]
        private ICollection<GoodsArticlesViewModel> articleList;

        [ObservableProperty]
        private ICollectionView articleCollection;

        private InventoryDocument inventoryDocument;


       [RelayCommand]
        public void MultipleEdit()
        {
            IsEnabled = true;
            inventoryDocument = new InventoryDocument
            {
                Created = DateTime.Now,
                Order = _categoryDataService.GetInventoryCounter().Result,
                Id = Guid.NewGuid(),
                StorageId = _storageService.GetStorageByName(StorageName).Result,
                SupplierId = null,
                Type = 2,
                IsActivated = true,
                IsDeleted = false
            };
            _categoryDataService.CreateInventoryDocument(inventoryDocument);

        }


        [RelayCommand]
        public void EditArticle(GoodsArticlesViewModel parameter)
        {
            IsEditOpen = true;
            this.EditArticleViewModel = new EditStorageViewModel(parameter, _notifier, _categoryDataService, _storageService, this, inventoryDocument);
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
        }


        [RelayCommand]
        public void SaveChanges()
        {

            int counter = 0;
            if(ListOfItems.Count > 0)
            {
                foreach (var item in ListOfItems)
                {
                    counter++;
                }
            }

            _notifier.ShowSuccess(counter + " corrections successfully saved.");
            ListOfItems.Clear();
            LoadData();
            IsEnabled = false;

        }

        [RelayCommand]
        public void DiscardChanges()
        {

            if(inventoryDocument == null)
            {
                _notifier.ShowInformation("Nothing to discard.");

            }
            else
            {
                try
                {
                    if (ListOfItems.Count == 1)
                    {
                        var inventoryItemId = ListOfItems[0].Id;

                        _categoryDataService.DeleteInventoryItem((Guid)inventoryItemId);

                    }
                    else if (ListOfItems.Count > 1)
                    {
                        var inventoryItemId = ListOfItems[0].Id;

                        foreach (var item in ListOfItems)
                        {
                            _categoryDataService.DeleteInventoryItem(item.Id);

                        }
                    }

                    _categoryDataService.DeleteInventoryDocument(inventoryDocument.Id);
                    IsEnabled = false;
                    _notifier.ShowSuccess("Successfully deleted!");
                }
                catch (Exception)
                {
                    _notifier.ShowError("An error occurred. Please try again.");
                    throw;
                }
            }
            

        }



        public async Task<ICollection<GoodsArticlesViewModel>> StorageQuantityCounter(string storageName)
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

            return await Task.FromResult(tempList);
        }


    }
}

