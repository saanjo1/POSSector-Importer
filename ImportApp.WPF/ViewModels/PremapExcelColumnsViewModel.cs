using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class PremapExcelColumnsViewModel
    {
        private ConcurrentDictionary<string, string> _myDictionary;
        private SettingsViewModel _settingsViewModel;

        [ObservableProperty]
        private ObservableCollection<string> someCollection;

        [ObservableProperty]
        private bool isOpened;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? price;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? storage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? barCode;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? order;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? category;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? quantity;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SetMultipleValuesCommand))]
        private string? subcategory;

        [ObservableProperty]
        public MultipleColumnNameSmallViewModel smallModal;


        public PremapExcelColumnsViewModel(ConcurrentDictionary<string, string> myDictionary, SettingsViewModel settingsViewModel)
        {
            _myDictionary = myDictionary;
            _settingsViewModel = settingsViewModel;
            LoadingData();
        }

        [RelayCommand]
        public void SetMultipleValues(string parameter)
        {
            IsOpened = true;
            this.SmallModal = new MultipleColumnNameSmallViewModel(_myDictionary, this, parameter);
        }


        [RelayCommand]
        public void Save()
        {
            _myDictionary.AddOrUpdate("Category", Category, (key, oldValue)=> Category);
            _myDictionary.AddOrUpdate("SubCategory", Subcategory, (key, oldValue) => Subcategory);
            _myDictionary.AddOrUpdate("Storage", Storage, (key, oldValue) => Storage);
            _myDictionary.AddOrUpdate("Name", Name, (key, oldValue) => Name);
            _myDictionary.AddOrUpdate("Price", Price, (key, oldValue) => Price);
            _myDictionary.AddOrUpdate("Quantity", Quantity, (key, oldValue) => Quantity);
            _myDictionary.AddOrUpdate("BarCode", BarCode, (key, oldValue) => BarCode);

            _settingsViewModel.Cancel();

            Console.WriteLine("Storage");
        }


        [RelayCommand]
        public void Cancel() {

            _settingsViewModel.Cancel();
        }


        public void LoadingData()
        {
            string currentName;
            string currentCat;
            string currentSubCat;
            string currentStorage;
            string currentPrice;
            string currentBarCode;
            string currentQuantity;

            if (_myDictionary.TryGetValue("Storage", out currentStorage))
            {
                Storage = currentStorage;
            }
            if(_myDictionary.TryGetValue("SubCategory", out currentSubCat))
            {
                Subcategory = currentSubCat;
            }
            if (_myDictionary.TryGetValue("Category", out currentCat))
            {
                Category = currentCat;
            }
            if (_myDictionary.TryGetValue("Name", out currentName))
            {
                Name = currentName;
            }
            if (_myDictionary.TryGetValue("BarCode", out currentBarCode))
            {
                BarCode = currentBarCode;
            }
            if (_myDictionary.TryGetValue("Quantity", out currentQuantity))
            {
                Quantity = currentQuantity;
            }
            if (_myDictionary.TryGetValue("Price", out currentPrice))
            {
                Price = currentPrice;
            }
        }
    }
}
