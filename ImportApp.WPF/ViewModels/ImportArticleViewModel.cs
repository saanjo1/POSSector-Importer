using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

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

        public ImportArticleViewModel(IExcelDataService excelDataService)
        {
            _excelDataService = excelDataService;
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

            }

        }

        [RelayCommand(CanExecute = nameof(CanImport))]
        public void ImportData()
        {

        }

        private bool CanImport()
=> !string.IsNullOrWhiteSpace(ExcelFile);

        [RelayCommand(CanExecute = nameof(CanMap))]
        public void MapData()
        {

        }

        private bool CanMap()
=> !string.IsNullOrWhiteSpace(ExcelFile);

    }
}
