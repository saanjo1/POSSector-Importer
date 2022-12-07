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
        private string excelFile;

        [ObservableProperty]
        public bool isOpen;

        [ObservableProperty]
        public bool isMapped;

        [ObservableProperty]
        private MapDataViewModel mDataModel;

        [ObservableProperty]
        private MapColumnViewModel mColumnModel;

        [ObservableProperty]
        private ICollectionView articleCollection;

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
            ArticleCollection = CollectionViewSource.GetDefaultView(vm);
            IsMapped = false;
        }

        [RelayCommand]
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
=> !string.IsNullOrWhiteSpace(ExcelFile);



        private bool CanMap()
=> !string.IsNullOrWhiteSpace(ExcelFile);

    }
}
