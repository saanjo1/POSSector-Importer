using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class ImportDataViewModel : BaseViewModel
    {

        private IExcelDataService _excelDataService;
        private Notifier _notifier;
        private ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        private ObservableCollection<ImportArticlesModalViewModel> articlesCollection = new ObservableCollection<ImportArticlesModalViewModel>();

        [ObservableProperty]
        private int count;

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
        private ImportArticlesModalViewModel articleQ;

        [ObservableProperty]
        private ICollectionView articleCollection;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearAllDataCommand))]
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

        private int currentPage = 1;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateEnableState();
            }
        }

        private int selectedRecord = 15;
        public int SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                UpdateRecordCount();
            }
        }

        private void UpdateRecordCount()
        {
            NumberOfPages = (int)Math.Ceiling((double)articleList.Count / SelectedRecord);
            NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;
            UpdateCollection(articleList.Take(SelectedRecord));
            CurrentPage = 1;
        }

        [ObservableProperty]
        private int numberOfPages = 15;

        [ObservableProperty]
        private bool isFirstEnabled;

        [ObservableProperty]
        private bool isPreviousEnabled;

        [ObservableProperty]
        private bool isNextEnabled;

        [ObservableProperty]
        private bool isLastEnabled;


        public static int RecordStartForm = 0;
        [RelayCommand]
        private void PreviousPage(object obj)
        {
            CurrentPage--;
            RecordStartForm = articleList.Count - SelectedRecord * (NumberOfPages - (CurrentPage - 1));

            var recordsToShow = articleList.Skip(RecordStartForm).Take(SelectedRecord);

            UpdateCollection(recordsToShow);
            UpdateEnableState();
        }

        [RelayCommand]
        private void LastPage(object obj)
        {
            var recordsToSkip = SelectedRecord * (NumberOfPages - 1);
            UpdateCollection(articleList.Skip(recordsToSkip));
            CurrentPage = NumberOfPages;
            UpdateEnableState();
        }


        [RelayCommand]
        private void FirstPage(object obj)
        {
            UpdateCollection(articleList.Take(SelectedRecord));
            CurrentPage = 1;
            UpdateEnableState();
        }

        [RelayCommand]
        private void NextPage(object obj)
        {
            RecordStartForm = CurrentPage * SelectedRecord;
            var recordsToShow = articleList.Skip(RecordStartForm).Take(SelectedRecord);
            UpdateCollection(recordsToShow);
            CurrentPage++;
            UpdateEnableState();
        }

        private void UpdateEnableState()
        {
            IsFirstEnabled = CurrentPage > 1;
            IsPreviousEnabled = CurrentPage > 1;
            IsNextEnabled = CurrentPage < NumberOfPages;
            IsLastEnabled = CurrentPage < NumberOfPages;
        }

        private void UpdateCollection(IEnumerable<ImportArticlesModalViewModel> recordsToShow)
        {
            ArticlesCollection.Clear();
            foreach (var item in recordsToShow)
            {
                ArticlesCollection.Add(item);
            }
        }


        [RelayCommand]
        public void LoadData(ObservableCollection<ImportArticlesModalViewModel>? vm = null)
        {
            if (vm != null)
            {
                articleList = vm;
                ArticleCollection = CollectionViewSource.GetDefaultView(vm);
            }
            //IsMapped = false;

            ArticleCollection = CollectionViewSource.GetDefaultView(ArticlesCollection);
            UpdateCollection(articlesCollection.Take(SelectedRecord));
            UpdateRecordCount();
            Count = ArticleList.Count;

        }
        [RelayCommand]
        public void LoadFixedExcelColumns()
        {
            ImportArticlesModalViewModel tempVm = new ImportArticlesModalViewModel();

            try
            {
                articleList = _excelDataService.ReadFromExcel(_myDictionary, tempVm).Result;
                _notifier.ShowInformation(articleList.Count() + " articles pulled. ");
                LoadData(articleList);
            }
            catch (Exception)
            {
                if (articleList == null)
                    _notifier.ShowError("Please check your ExcelFile & Sheet, and try again.");
                else
                    _notifier.ShowError(Translations.ErrorMessage);

            }

        }
        [RelayCommand]
        public void ClearAllData()
        {
            if (articleList != null)
            {
                articleList.Clear();
                ArticleCollection = null;
            }
            else
            {
                _notifier.ShowError("Can not clear empty list.");
            }
        }

        [RelayCommand]
        public void ImportData()
        {

            if (articleList != null)
            {
                Guid _supplierId = _supplierDataService.GetSupplierByName("YAMMAMAY").Result;

                List<string> _storages = new List<string>();
                List<InventoryDocument> documents = new List<InventoryDocument>();

                for (int i = 0; i < articleList.Count; i++)
                {
                    if (!_storages.Contains(articleList[i].Storage))
                        _storages.Add(articleList[i].Storage);
                }

                foreach (var item in _storages)
                {
                    InventoryDocument inventoryDocument = new InventoryDocument
                    {
                        Created = DateTime.Now,
                        Order = _categoryService.GetInventoryCounter().Result,
                        Id = Guid.NewGuid(),
                        StorageId = _storageService.GetStorageByName(item).Result,
                        SupplierId = _supplierId,
                        Type = 1,
                        IsActivated = false,
                        IsDeleted = false
                    };

                    _categoryService.CreateInventoryDocument(inventoryDocument);
                    documents.Add(inventoryDocument);
                }

                int counter = _articleService.GetLastArticleNumber().Result;
                try
                {

                    if (ArticleList.Count > 0 || ArticleList != null)
                    {

                        for (int i = 0; i < articleList.Count; i++)
                        {
                            InventoryDocument inventoryDocument = GetCreatedInventoryDocument(articleList[i].Storage, documents);
                            Guid _goodId = _articleService.GetGoodId(articleList[i].Name).Result;

                            if (_goodId == Guid.Empty)
                            {
                                Good newGood = new Good
                                {
                                    Id = Guid.NewGuid(),
                                    Name = articleList[i].Name,
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
                                StorageId = inventoryDocument.StorageId,
                                GoodId = _goodId,
                                CurrentQuantity = Helpers.Extensions.GetDecimal(articleList[i].Quantity)
                            };

                            _categoryService.CreateInventoryItem(inventoryItemBasis);

                            Guid? ArticleGuid = _articleService.GetArticleByName(articleList[i].Name).Result;

                            if (ArticleGuid == Guid.Empty)
                            {
                                Article newArticle = new Article
                                {
                                    Id = Guid.NewGuid(),
                                    Name = articleList[i].Name,
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

                    if (articleList.Count > 0)
                    {
                        _notifier.ShowSuccess("Storage successfully updated");
                    }
                    else
                        _notifier.ShowWarning("Storage update works, but you need to provide records first.");

                    articleList.Clear();
                    ArticleCollection = null;
                }
                catch (Exception)
                {
                    _notifier.ShowError(Translations.ErrorMessage);

                    throw;
                }
            }
            else
            {
                _notifier.ShowError("Can not import an empty list.");
            }
        }

        private InventoryDocument GetCreatedInventoryDocument(string? storage, List<InventoryDocument> documents)
        {
            Guid _storageId = _storageService.GetStorageByName(storage).Result;

            foreach (var item in documents)
            {
                if (item.StorageId == _storageId)
                    return item;
            }

            return null;
        }


        [RelayCommand]
        private void DeleteArticle(ImportArticlesModalViewModel parameter)
        {
            try
            {
                var deletedArticle = articleList.Remove(parameter);
                _notifier.ShowSuccess("Article successfully deleted.");
                LoadData();
            }
            catch (Exception)
            {
                _notifier.ShowError("An error occurred while deleting article.");
                throw;
            }
        }


        public void MapData()
        {
            //this.IsMapped = true;
            this.ArticleQ = new ImportArticlesModalViewModel(this, _excelDataService, _myDictionary, _notifier, _supplierDataService);
        }

        public void Close()
        {
            if (articleQ != null)
            {
                articleQ = null;
                //IsMapped = false;
            }
        }




        //private bool CanMap()
        //{
        //    if (_myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value) == false)
        //    {
        //        _notifier.ShowWarning(Translations.SettingsError);
        //        return false;
        //    }
        //    return true;
        //}



    }
}
