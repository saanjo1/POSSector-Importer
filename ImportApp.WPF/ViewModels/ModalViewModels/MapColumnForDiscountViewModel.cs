using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnForDiscountViewModel : BaseViewModel
    {
        private Notifier _notifier;
        public IExcelDataService? _excelDataService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private bool isrbOption2Checked;

        [ObservableProperty]
        private bool isrbOption1Checked;


        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? category;

        [ObservableProperty]
        private string? storage;

        [ObservableProperty]
        private string? barCode;

        [ObservableProperty]
        private string? price;

        [ObservableProperty]
        private string? discount;

        [ObservableProperty]
        private string? newPrice;

        [ObservableProperty]
        MapColumnForDiscountViewModel selectedItems;

        [ObservableProperty]
        ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        ObservableCollection<MapColumnForDiscountViewModel>? articleList;


        [ObservableProperty]
        List<string> columnNames;
        
        private DiscountViewModel _discountViewModel;

        public MapColumnForDiscountViewModel(DiscountViewModel discountViewModel, IExcelDataService? excelDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier)
        {
            _discountViewModel = discountViewModel;
            _excelDataService = excelDataService;
            _myDictionary = myDictionary;
            LoadColumnNames();
            _notifier = notifier;
            SelectedItems = Helpers.Extensions.SelectedColumns(this, ColumnNames, myDictionary);
        }

        public MapColumnForDiscountViewModel()
        {

        }

        [RelayCommand]
        public void CloseModal()
        {
            _discountViewModel.Close();        
        
        }

        [RelayCommand]
        public void Submit()
        {
            try
            {
                ObservableCollection<MapColumnForDiscountViewModel>? excelDataList;
                excelDataList = _excelDataService.ReadFromExcel(_myDictionary, this).Result;
                _notifier.ShowInformation(excelDataList.Count() + " articles pulled. ");
                _discountViewModel.LoadData(excelDataList);
            }
            catch (Exception)
            {
                _notifier.ShowError("Please check your input and try again.");
            }
        }


        public void LoadColumnNames()
        {
            ColumnNames = _excelDataService.ListColumnNames(_myDictionary[Translations.CurrentExcelSheet]).Result;
        }

      

    }
}
