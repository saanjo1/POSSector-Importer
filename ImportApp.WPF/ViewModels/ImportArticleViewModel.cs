using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ImportApp.Domain;
using ImportApp.Domain.Models;
using ImportApp.EntityFramework.DBContext;
using ImportApp.Domain.Services;
using CustomMessageBox;
using System.Diagnostics.Metrics;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class ImportArticleViewModel : BaseViewModel
    {
       
        private IExcelDataService _excelDataService;

        private IArticleDataService _articleService;
        private ICategoryDataService _categoryService;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenDialogCommand))]
        private string excelFile;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenDialogCommand))]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        private bool isOpen;   
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenDialogCommand))]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        private bool isMapped;

        [ObservableProperty]
        private MapDataViewModel mDataModel;

        [ObservableProperty]
        private MapColumnViewModel mColumnModel;

        [ObservableProperty]
        private ICollectionView articleCollection;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ImportDataCommand))]
        ObservableCollection<MapColumnViewModel>? articleList;

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
                var filt = obj as MapColumnViewModel;
                return filt != null && (filt.Name.Contains(TextToFilter) || filt.BarCode.Contains(TextToFilter) || filt.Price.ToString() == TextToFilter || filt.ArticleNumber.ToString() == TextToFilter);
            }
            return true;
        }


        public ImportArticleViewModel(IExcelDataService excelDataService, ICategoryDataService categoryService, IArticleDataService articleService)
        {
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _articleService = articleService;
        }

        [RelayCommand]
        public void LoadData(ObservableCollection<MapColumnViewModel>? vm)
        {
           if(vm != null)
            {
                articleList = vm;
                ArticleCollection = CollectionViewSource.GetDefaultView(vm);
            }
            IsMapped = false;

        }

        [RelayCommand(CanExecute = nameof(CanUpload))]
        public void OpenDialog()
        {
            try
            {
                ExcelFile = _excelDataService.OpenDialog().Result;
                
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        [RelayCommand(CanExecute = nameof(CanImport))]
        public void ImportData()
        {
            var counter = 0;
            try
            {
                for (int i = 0; i < articleList.Count; i++)
                {
                    var value = articleList[i].BarCode;
                    Article temp = _articleService.Compare(value).Result;

                    if(temp == null)
                    {
                        counter++;
                        Article newArticle = new Article()
                        {
                            Id = Guid.NewGuid(),
                            Name = articleList[i].Name,
                            ArticleNumber = 123456789,
                            Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
                            BarCode = articleList[i].BarCode,
                            SubCategoryId = _categoryService.ManageSubcategories(articleList[i].Gender, articleList[i].Collection).Result,
                            Deleted = false,
                            Order = 1
                        };

                        _articleService.Create(newArticle);
                    }
                }
                bool? Result = new MessageBoxCustom(counter + " articles imported.", MessageType.Success, MessageButtons.Ok).ShowDialog();

            }
            catch (Exception)
            {
                bool? _rez = new MessageBoxCustom("Please check your input and try again.", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }

        [RelayCommand(CanExecute = nameof(CanMap))]
        public void MapData()
        {
            IsOpen = true;
            this.MDataModel = new MapDataViewModel(_excelDataService, this, mColumnModel);
        }

        public void Close()
        {
            if(MDataModel != null)
            {
                MDataModel = null;
                IsOpen = false;
            }

            if(MColumnModel != null)
            {
                MColumnModel = null;
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
            if (string.IsNullOrWhiteSpace(ExcelFile) || IsOpen == true || IsMapped == true)
            {
                return false;
            }
            return true;
        }


        private bool CanUpload()
        {
            if (IsMapped == true || IsOpen == true)
            {
                return false;
            }
            return true;
        }
    }
}
