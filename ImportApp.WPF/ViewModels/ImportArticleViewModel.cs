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
        private string excelFile;

        public ImportArticleViewModel(IExcelDataService excelDataService)
        {
            _excelDataService = excelDataService;
        }

        public bool cExecute() => ExcelFile != null;

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
    }
}
