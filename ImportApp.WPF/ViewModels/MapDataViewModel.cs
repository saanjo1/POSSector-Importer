using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.EntityFramework.Services;
using ModalControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapDataViewModel : BaseViewModel
    {

        [ObservableProperty]
        private List<string> currentSheets;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedSheet;

        private IExcelDataService _excelDataService;
        private readonly ImportArticleViewModel _importArticleViewModel;


        [ObservableProperty]
        private MapColumnViewModel? mColumnModel;

        [ObservableProperty]
        private bool isMapped;

        public MapDataViewModel(IExcelDataService excelDataService, ImportArticleViewModel importArticleViewModel, MapColumnViewModel mapColumnViewModel)
        {
            _excelDataService = excelDataService;
            mColumnModel = mapColumnViewModel;
            _importArticleViewModel = importArticleViewModel;
            CurrentSheets = _excelDataService.ListSheetsFromFile().Result;

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
                List<string>? columnNamesList = _excelDataService.ListColumnNames(SelectedSheet).Result;

                MapColumnViewModel mColumnModel = new MapColumnViewModel(_excelDataService, SelectedSheet, columnNamesList, _importArticleViewModel);
                IsMapped = true;
                _importArticleViewModel.IsMapped = true;
                _importArticleViewModel.IsOpen = false;
                _importArticleViewModel.MColumnModel = mColumnModel;
            }
        }


        private bool CanSave()
=> !string.IsNullOrWhiteSpace(SelectedSheet);
    }
}
