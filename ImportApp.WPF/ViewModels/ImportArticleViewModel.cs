using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using Microsoft.Win32;
using ModalControl;
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

        [ObservableProperty]
        public bool isOpen;

        [ObservableProperty]
        private MapDataViewModel mDataModel;

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
            this.MDataModel = new MapDataViewModel(_excelDataService);
        }


        private bool CanImport()
=> !string.IsNullOrWhiteSpace(ExcelFile);



        private bool CanMap()
=> !string.IsNullOrWhiteSpace(ExcelFile);

    }
}
