using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnViewModel : BaseViewModel
    {
        public static string SheetName;


        public static IExcelDataService _excelDataService;

        [ObservableProperty]
        private List<string> columnNamesList;

        private ImportArticleViewModel _viewModel;


        public MapColumnViewModel(IExcelDataService excelDataService, string sheetName, List<string> _columnNames, ImportArticleViewModel viewModel)
        {
            _excelDataService = excelDataService;
            columnNamesList = _columnNames;
            SheetName = sheetName;
            _viewModel = viewModel;
        }


        [RelayCommand]
        public void Cancel()
        {
            _viewModel.Close();
        }


    }
}
