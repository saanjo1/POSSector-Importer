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


        public ImportArticleViewModel(IExcelDataService excelDataService, ICategoryDataService categoryService, IArticleDataService articleService)
        {
            _excelDataService = excelDataService;
            _categoryService = categoryService;
            _articleService = articleService;
        }


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
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenDialogCommand))]
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
                return filt != null && (filt.Name.Contains(TextToFilter) || filt.BarCode.Contains(TextToFilter) || filt.Price.ToString() == TextToFilter || filt.Price.ToString() == TextToFilter);
            }
            return true;
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

                if(ExcelFile != null)
                {
                    bool? Result = new MessageBoxCustom("Excel file selected.", MessageType.Info, MessageButtons.Ok).ShowDialog();
                }

            }
            catch (System.Exception)
            {
                bool? Result = new MessageBoxCustom("An error occured while selecting file.", MessageType.Error, MessageButtons.Ok).ShowDialog();

            }

        }

        [RelayCommand(CanExecute = nameof(CanImport))]
        public void ImportData()
        {
            var counter = 0;
            var updatedCounter = 0;
            try
            {
                for (int i = 0; i < articleList.Count; i++)
                {
                    var value = articleList[i].BarCode;
                    Article temp = _articleService.Compare(value).Result;
                    Article newArticle = new Article()
                    {
                        Id = Guid.NewGuid(),
                        Name = articleList[i].Name,
                        ArticleNumber = 123456789,
                        Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
                        BarCode = articleList[i].BarCode,
                        SubCategoryId = _categoryService.ManageSubcategories(articleList[i].Gender, articleList[i].Collection, articleList[i].Storage).Result,
                        Deleted = false,
                        Order = 1,
                    };

                    if (temp == null)
                    {
                        counter++;
                        _articleService.Create(newArticle);
                    } else
                    {
                        updatedCounter++;
                        _articleService.Update(temp.Id, newArticle);
                    }
                }
                bool? Result = new MessageBoxCustom(counter + " articles imported. " + updatedCounter + " articles updated.", MessageType.Success, MessageButtons.Ok).ShowDialog();

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
            if (string.IsNullOrWhiteSpace(ExcelFile) || IsOpen == true || IsMapped == true || articleList != null)
            {
                return false;
            }
            return true;
        }


        private bool CanUpload()
        {
            if (IsMapped == true || IsOpen == true || articleList != null)
            {
                return false;
            }
            return true;
        }
    }
}
