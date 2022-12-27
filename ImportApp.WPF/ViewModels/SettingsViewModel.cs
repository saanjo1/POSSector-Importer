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
    public partial class SettingsViewModel : BaseViewModel
    {

        public SettingsViewModel() { }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateRuleCommand))]
        private bool isOpen;

        [ObservableProperty]
        private CreateNewDiscountViewModel ruleViewModel;



        [RelayCommand]
        public void CreateRule()
        {
            IsOpen = true;
            this.RuleViewModel = new CreateNewDiscountViewModel(this);
        }

        [RelayCommand]
        public void Cancel()
        {
            IsOpen = false;
        }

    }
}
