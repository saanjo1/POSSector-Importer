using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Collections.Concurrent;
using ImportApp.WPF.Resources;
using Microsoft.Identity.Client.Extensions.Msal;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class ImportDataViewModel : BaseViewModel
    {
       
        private IExcelDataService _excelDataService;
        private Notifier _notifier;
        private ConcurrentDictionary<string, string> _myDictionary;

        private IArticleDataService _articleService;
        private ICategoryDataService _categoryService;
        private IStorageDataService _storageService;
        private ISupplierDataService _supplierDataService;


        public ImportDataViewModel(IExcelDataService excelDataService, ICategoryDataService categoryService, IArticleDataService articleService, Notifier notifier, ConcurrentDictionary<string, string> myDictionary, IStorageDataService storageService, ISupplierDataService supplierDataService)
        {
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _articleService = articleService;
            _notifier = notifier;
            _myDictionary = myDictionary;
            _storageService = storageService;
            _supplierDataService = supplierDataService;
        }



        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        private bool isOpen;   
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        private bool isMapped;


        [ObservableProperty]
        private ImportArticlesModalViewModel articleQ;

        [ObservableProperty]
        private ICollectionView articleCollection;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        ObservableCollection<ImportArticlesModalViewModel>? articleList;

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
                var filt = obj as ImportArticlesModalViewModel;
                return filt != null && filt.BarCode.Contains(TextToFilter);
            }
            return true;
        }



        [RelayCommand]
        public void LoadData(ObservableCollection<ImportArticlesModalViewModel>? vm)
        {
           if(vm != null)
            {
                articleList = vm;
                ArticleCollection = CollectionViewSource.GetDefaultView(vm);
            }
            IsMapped = false;

        }

        

        [RelayCommand(CanExecute = nameof(CanImport))]
        public void ImportData()
        {

            Guid storageId = _storageService.GetStorageByName(articleList[0].Storage).Result;
            Guid _supplierId = _supplierDataService.GetSupplierByName(articleList[0].Supplier).Result;
            int counter = _articleService.GetLastArticleNumber().Result;

            try
            {
                InventoryDocument inventoryDocument = new InventoryDocument
                {
                    Created = DateTime.Now,
                    Order = 1,
                    Id = Guid.NewGuid(),
                    StorageId = storageId,
                    SupplierId = _supplierId,
                    Type = 1,
                    IsActivated = false,
                    IsDeleted = false
                };

                _categoryService.CreateInventoryDocument(inventoryDocument);


                if (ArticleList.Count > 0 || ArticleList != null)
                {

                    for (int i = 0; i < articleList.Count; i++)
                    {
                        Guid _goodId = _articleService.GetGoodId(articleList[i].Name + " " + articleList[i].BarCode).Result;

                        if (_goodId == Guid.Empty)
                        {
                            Good newGood = new Good
                            {
                                Id = Guid.NewGuid(),
                                Name = articleList[i].Name + " " + articleList[i].BarCode,
                                UnitId = _articleService.GetUnitByName("kom").Result,
                                LatestPrice = Helpers.Extensions.GetDecimal(articleList[i].PricePerUnit),
                                Volumen = 1,
                                Refuse = 0
                            };

                            _categoryService.CreateGood(newGood);

                            _goodId = newGood.Id;
                        }


                        InventoryItemBasis inventoryItemBasis = new InventoryItemBasis
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.Now,
                            Price = Helpers.Extensions.GetDecimal(articleList[i].PricePerUnit),
                            Quantity = Helpers.Extensions.GetDecimal(articleList[i].Quantity),
                            Total = Helpers.Extensions.GetDecimal(articleList[i].PricePerUnit) * Helpers.Extensions.GetDecimal(articleList[i].Quantity),
                            Tax = 0,
                            IsDeleted = false,
                            Discriminator = "InventoryDocumentItem",
                            InventoryDocumentId = inventoryDocument.Id,
                            StorageId = storageId,
                            GoodId = _goodId,
                            CurrentQuantity = Helpers.Extensions.GetDecimal(articleList[i].Quantity)
                        };

                        _categoryService.CreateInventoryItem(inventoryItemBasis);

                        Guid? ArticleGuid = _articleService.GetArticleByName(articleList[i].Name + " " + articleList[i].BarCode).Result;

                        if (ArticleGuid == Guid.Empty)
                        {
                            Article newArticle = new Article
                            {
                                Id = Guid.NewGuid(),
                                Name = articleList[i].Name + " " + articleList[i].BarCode,
                                ArticleNumber = _articleService.GetLastArticleNumber().Result,
                                Order = 1,
                                SubCategoryId = _categoryService.ManageSubcategories(articleList[i].Category, articleList[i].Storage).Result,
                                BarCode = articleList[i].BarCode,
                                Price = Helpers.Extensions.GetDecimal(articleList[i].Price)
                            };

                            _articleService.Create(newArticle);

                            ArticleGood newArticleGood = new ArticleGood
                            {
                                Id = Guid.NewGuid(),
                                ArticleId = newArticle.Id,
                                GoodId = _goodId,
                                Quantity = 1,
                                ValidFrom = DateTime.Today,
                                ValidUntil = DateTime.Today.AddDays(5)
                            };

                            _categoryService.CreateArticleGood(newArticleGood);

                        }
                        else
                        {
                            ArticleGood newArticleGood = new ArticleGood
                            {
                                Id = Guid.NewGuid(),
                                ArticleId = ArticleGuid,
                                GoodId = _goodId,
                                Quantity = 1,
                                ValidFrom = DateTime.Today,
                                ValidUntil = DateTime.Today.AddDays(5)
                            };

                            _categoryService.CreateArticleGood(newArticleGood);

                        }

                    }
                }

                _notifier.ShowSuccess("Storage successfully updated");
            }
            catch (Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);

                throw;
            }
        }

        [RelayCommand(CanExecute = nameof(CanMap))]
        public void MapData()
        {
            this.IsMapped = true;
            this.ArticleQ = new ImportArticlesModalViewModel(this, _excelDataService, _myDictionary, _notifier, _supplierDataService);
        }

        public void Close()
        {
            if(articleQ != null)
            {
                articleQ = null;
                IsMapped = false;
            }
        }

        private bool CanImport()
        {
            if (IsMapped == true || IsOpen == true || articleList == null)
            {
                return false;
            }
            return true;
        }


        private bool CanMap()
        {
            if (_myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value) == false)
            {
                _notifier.ShowWarning(Translations.SettingsError);
                return false;
            }
            return true;
        }


      
    }
}
