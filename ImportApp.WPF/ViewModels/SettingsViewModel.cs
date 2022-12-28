using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class SettingsViewModel : BaseViewModel
    {

        private IDiscountDataService _discountDataService;
        private ConcurrentDictionary<string, string> _myDictionary;


        public SettingsViewModel(IDiscountDataService discountDataService, ConcurrentDictionary<string, string> myDictionary)
        {
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateRuleCommand))]
        private bool isOpen;

        [ObservableProperty]
        private CreateNewDiscountViewModel ruleViewModel;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateMapRuleCommand))]
        private bool isEnableToMap;

        [ObservableProperty]
        private PremapExcelColumnsViewModel preMapRuleViewModel;



        [RelayCommand]
        public void CreateRule()
        {
            IsOpen = true;
            this.RuleViewModel = new CreateNewDiscountViewModel(this, _discountDataService);
        }

        [RelayCommand]
        public void CreateMapRule()
        {
            IsEnableToMap = true;
            this.PreMapRuleViewModel = new PremapExcelColumnsViewModel(_myDictionary);
        }

        [RelayCommand]
        public void Cancel()
        {
            IsOpen = false;
        }

    }
}
