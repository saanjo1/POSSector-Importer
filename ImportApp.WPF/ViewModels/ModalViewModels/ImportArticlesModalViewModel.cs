using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    public partial class ImportArticlesModalViewModel : ObservableObject
    {
        private Notifier _notifier;
        public IExcelDataService? _excelDataService;
        public ISupplierDataService? _supplierDataService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private bool isOption1Checked;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private bool isOption2Checked;


        [ObservableProperty]
        private string? name;


        [ObservableProperty]
        private string? category;

        [ObservableProperty]
        private string? supplier;

        [ObservableProperty]
        private string? storage;

        [ObservableProperty]
        private string? barCode;

        [ObservableProperty]
        private string? price;

        [ObservableProperty]
        private string? quantity;

        [ObservableProperty]
        private string? pricePerUnit;


        [ObservableProperty]
        ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        ObservableCollection<MapColumnForDiscountViewModel>? articleList;


        [ObservableProperty]
        List<string> columnNames;


        [ObservableProperty]
        List<string> suppliersList;


        private ImportDataViewModel _importDataViewModel;

        public ImportArticlesModalViewModel(ImportDataViewModel importDataViewModel, IExcelDataService? excelDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier, ISupplierDataService? supplierDataService)
        {
            _importDataViewModel = importDataViewModel;
            _excelDataService = excelDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
            _supplierDataService = supplierDataService;
            LoadColumnNames();
        }

        public ImportArticlesModalViewModel()
        {

        }

        [RelayCommand]
        public void CloseModal()
        {
            _importDataViewModel.Close();

        }

        [RelayCommand]
        public void Submit()
        {
            try
            {
                ObservableCollection<ImportArticlesModalViewModel>? excelDataList;
                excelDataList = _excelDataService.ReadFromExcel(_myDictionary, this).Result;
                _notifier.ShowInformation(excelDataList.Count() + " articles pulled. ");
                _importDataViewModel.LoadData(excelDataList);
            }
            catch (Exception)
            {
                _notifier.ShowError("Please check your input and try again.");
            }
        }


        public void LoadColumnNames()
        {
            ColumnNames = _excelDataService.ListColumnNames(_myDictionary[Translations.CurrentExcelSheet]).Result;
            SuppliersList = _supplierDataService.GetListOfSuppliers().Result;

            if (SuppliersList != null)
                Supplier = SuppliersList[0];
        }


    }
}
