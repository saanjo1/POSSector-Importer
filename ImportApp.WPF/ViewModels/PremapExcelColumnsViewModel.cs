using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private ObservableCollection<string> someCollection;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public MultipleColumnNameSmallViewModel smallModal;

        [ObservableProperty]
        public bool isOpened;

        public PremapExcelColumnsViewModel(ConcurrentDictionary<string, string> myDictionary)
        {
            _myDictionary = myDictionary;
        }

        [RelayCommand]
        public void SetMultipleValues()
        {
            IsOpened = true;
            this.SmallModal = new MultipleColumnNameSmallViewModel(_myDictionary, this);
        }


        [RelayCommand]
        public void Close() {

           IsOpened = false;
        }
    }
}
