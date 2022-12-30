using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ModalControl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapDataViewModel : BaseViewModel
    {
        private Notifier _notifier;


        [ObservableProperty]
        private List<string> currentSheets;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedSheet;

        private IExcelDataService _excelDataService;
        private readonly ImportDataViewModel _importArticleViewModel;
        private ConcurrentDictionary<string, string> _myDictionary;


        [ObservableProperty]
        private MapColumnViewModel? mColumnModel;

        [ObservableProperty]
        private bool isMapped;

        public MapDataViewModel(IExcelDataService excelDataService, ImportDataViewModel importArticleViewModel, MapColumnViewModel mapColumnViewModel, Notifier notifier, ConcurrentDictionary<string, string> myDictionary)
        {
            _excelDataService = excelDataService;
            mColumnModel = mapColumnViewModel;
            _importArticleViewModel = importArticleViewModel;
            CurrentSheets = _excelDataService.ListSheetsFromFile().Result;
            SelectedSheet = CurrentSheets[0];
            _notifier = notifier;
            _myDictionary = myDictionary;
        }

        [RelayCommand]
        public void Cancel()
        {
            _importArticleViewModel.Close();
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            if (SelectedSheet != null)
            {
                ObservableCollection<MapColumnViewModel>? excelDataList;
                App.Current.Properties["SheetName"] = SelectedSheet;
                excelDataList = _excelDataService.ReadFromExcel(_myDictionary).Result;
                _notifier.ShowInformation(excelDataList.Count() + " articles pulled. ");
                _importArticleViewModel.LoadData(excelDataList);
                _importArticleViewModel.Close();
            }
        }


        private bool CanSave()
=> !string.IsNullOrWhiteSpace(SelectedSheet);
    }
}
