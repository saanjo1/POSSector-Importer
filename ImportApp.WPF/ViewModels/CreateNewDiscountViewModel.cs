using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewDiscountViewModel
    {

        private readonly SettingsViewModel _settings;


        public CreateNewDiscountViewModel(SettingsViewModel settings)
        {
            _settings = settings;
        }

        [ObservableProperty]
        private bool isOpen;


        [RelayCommand]
        public void Cancel()
        {
            _settings.Cancel();
        }
    }
}
