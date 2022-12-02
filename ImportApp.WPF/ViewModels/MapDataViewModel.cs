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
        private string selectedSheet;

        private IExcelDataService _excelDataService;
        private readonly ImportArticleViewModel _importArticleViewModel;

        public MapDataViewModel(IExcelDataService excelDataService, ImportArticleViewModel importArticleViewModel)
        {
            _excelDataService = excelDataService;
            _importArticleViewModel = importArticleViewModel;
            CurrentSheets = _excelDataService.ListSheetsFromFile().Result;
        }

        [RelayCommand]
        public void Cancel()
        {
            _importArticleViewModel.Close();
        }

        [RelayCommand]
        public void Submit()
        {
            
        }






    }
}
