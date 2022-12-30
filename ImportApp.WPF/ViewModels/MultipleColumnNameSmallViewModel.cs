using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MultipleColumnNameSmallViewModel
    {
        [ObservableProperty]
        private string newColumn;

        private ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyCanExecuteChangedFor(nameof(ClearCurrentCommand))]
        private PremapExcelColumnsViewModel settingsModel;


        [ObservableProperty]
        private string parameterName;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewColumnCommand))]
        [NotifyCanExecuteChangedFor(nameof(ClearCurrentCommand))]
        private string currentFormOfName;

        public MultipleColumnNameSmallViewModel(ConcurrentDictionary<string, string> myDictionary, PremapExcelColumnsViewModel viewModel, string _parameterName)
        {
            _myDictionary = myDictionary;
            SettingsModel = viewModel;
            parameterName = _parameterName;
            string value;
            if(myDictionary.TryGetValue(parameterName, out value))
            {
                CurrentFormOfName = value;
            }
        }

        [RelayCommand]
        public void AddNewColumn(string newCol)
        {
            if (CurrentFormOfName == null)
                CurrentFormOfName = newCol;
            else
                CurrentFormOfName += " ; " + newCol;
        }


        [RelayCommand]
        public void Save()
        {
                _myDictionary.AddOrUpdate(parameterName, CurrentFormOfName, (key, oldValue) => CurrentFormOfName);
                SettingsModel.LoadingData();

        }

        [RelayCommand]
        public void ClearCurrent()
        {
            if (CurrentFormOfName != null)
            {
                _myDictionary.TryRemove(parameterName, out string value);
                CurrentFormOfName = null;
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            SettingsModel.Cancel();
        }
    }
}
