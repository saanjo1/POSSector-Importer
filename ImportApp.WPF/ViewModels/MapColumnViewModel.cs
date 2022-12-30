using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnViewModel : BaseViewModel
    {
        public static string SheetName;
        private Notifier _notifier;
        public static IExcelDataService? _excelDataService;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? price;

        [ObservableProperty]
        private string? storage;

        [ObservableProperty]
        private string? barCode;

        [ObservableProperty]
        private string? order;

        [ObservableProperty]
        private string? gender;

        [ObservableProperty]
        private string? quantity;

        [ObservableProperty]
        private string? collection;

        [ObservableProperty]
        private List<string> columnNamesList;

        private ImportDataViewModel _viewModel;


        public MapColumnViewModel(IExcelDataService excelDataService, string sheetName, List<string> _columnNames, ImportDataViewModel viewModel, Notifier notifier)
        {
            _excelDataService = excelDataService;
            columnNamesList = _columnNames;
            SheetName = sheetName;
            _viewModel = viewModel;
            _notifier = notifier;
        }

        public MapColumnViewModel()
        {
            
        }

        [RelayCommand]
        public void CloseModal()
        {
            _viewModel.Close();        
        
        }

        [RelayCommand]
        public void Submit()
        {
            //try
            //{
            //    ObservableCollection<MapColumnViewModel>? excelDataList;
            //    excelDataList = _excelDataService.ReadFromExcel(this).Result;
            //    _notifier.ShowInformation(excelDataList.Count() + " articles pulled. ");
            //    _viewModel.LoadData(excelDataList);
            //}
            //catch (Exception)
            //{
            //    _notifier.ShowError("Please check your input and try again.");
            //}
        }


    }
}
