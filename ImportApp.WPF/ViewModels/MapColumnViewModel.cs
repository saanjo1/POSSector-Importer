using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnViewModel : BaseViewModel
    {
        public static string SheetName;
        public static IExcelDataService? _excelDataService;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? price;

        [ObservableProperty]
        private string? articleNumber;

        [ObservableProperty]
        private string? barCode;

        [ObservableProperty]
        private string? order;

        [ObservableProperty]
        private string? gender;

        [ObservableProperty]
        private string? quantity;

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

        public MapColumnViewModel()
        {

        }

        [RelayCommand]
        public void Cancel()
        {
            _viewModel.Close();
        }

        [RelayCommand]
        public void Submit()
        {
           ObservableCollection<MapColumnViewModel>? excelDataList = _excelDataService.ReadFromExcel(this).Result;
            _viewModel.LoadData(excelDataList);
        }


    }
}
