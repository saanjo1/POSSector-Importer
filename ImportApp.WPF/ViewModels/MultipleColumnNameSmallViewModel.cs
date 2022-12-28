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
        private PremapExcelColumnsViewModel _viewModel;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewColumnCommand))]
        [NotifyCanExecuteChangedFor(nameof(ClearCurrentCommand))]
        private string currentFormOfName;

        public MultipleColumnNameSmallViewModel(ConcurrentDictionary<string, string> myDictionary, PremapExcelColumnsViewModel viewModel)
        {
            _myDictionary = myDictionary;
            _viewModel = viewModel;
            string value;
            if(myDictionary.TryGetValue("Name", out value))
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
                CurrentFormOfName += " + " + newCol;
        }


        [RelayCommand]
        public void Save()
        {
            if(CurrentFormOfName != null)
            {
                _myDictionary.TryAdd("Name", CurrentFormOfName);
                Cancel();
            }
        }

        [RelayCommand]
        public void ClearCurrent()
        {
            if (CurrentFormOfName != null)
            {
                _myDictionary.Clear();
                CurrentFormOfName = null;
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            _viewModel.Close();
        }
    }
}
