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


        public ImportDataViewModel(IExcelDataService excelDataService, ICategoryDataService categoryService, IArticleDataService articleService, Notifier notifier, ConcurrentDictionary<string, string> myDictionary)
        {
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _articleService = articleService;
            _notifier = notifier;
            _myDictionary = myDictionary;
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
            //var counter = 0;
            //var updatedCounter = 0;
            //try
            //{
            //    for (int i = 0; i < articleList.Count; i++)
            //    {
            //        string? value = articleList[i].BarCode;
            //        Article temp = _articleService.Compare(value).Result;
            //        Article newArticle = new Article()
            //        {
            //            Id = Guid.NewGuid(),
            //            Name = articleList[i].Name,
            //            ArticleNumber = i,
            //            Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
            //            BarCode = articleList[i].BarCode,
            //            SubCategoryId = _categoryService.ManageSubcategories(articleList[i]?.Gender, articleList[i]?.Collection, articleList[i]?.Storage).Result,
            //            Deleted = false,
            //            Order = 1,
            //        };

            //        if (temp == null)
            //        {
            //            counter++;
            //            if (_articleService.Create(newArticle).Result)
            //            {
            //                ArticleGood newArticleGood = new ArticleGood()
            //                {
            //                    ArticleId = newArticle.Id,
            //                    Id = Guid.NewGuid(),
            //                    Quantity = Helpers.Extensions.GetDecimal(articleList[i]?.Quantity.ToString()),
            //                    GoodId = null,
            //                    ValidFrom = DateTime.Now,
            //                    ValidUntil = DateTime.Now
            //                };

            //                _articleService.ManageArticleGood(newArticleGood);
            //            }
            //        }
            //        else
            //        {
            //            _articleService.Update(temp.Id, newArticle);
            //            updatedCounter++;

            //        }
            //    }
            //    articleList.Clear();
            //    _notifier.ShowSuccess(counter + " article(s) imported. " + updatedCounter + " article(s) updated.");

            //}
            //catch (Exception)
            //{
            //    _notifier.ShowError("Your input is not valid. Please try again.");
            //}
        }

        [RelayCommand(CanExecute = nameof(CanMap))]
        public void MapData()
        {
            this.IsMapped = true;
            this.ArticleQ = new ImportArticlesModalViewModel(this, _excelDataService, _myDictionary, _notifier);
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
