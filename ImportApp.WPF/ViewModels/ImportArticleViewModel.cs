using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using Microsoft.Win32;
using ModalControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class ImportArticleViewModel : BaseViewModel
    {
       
        private IExcelDataService _excelDataService;

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


        public ImportArticleViewModel(IExcelDataService excelDataService)
        {
            _excelDataService = excelDataService;
            
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
            if (IsMapped == true || IsOpen == true)
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
