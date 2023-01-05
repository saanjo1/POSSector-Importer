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
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class SettingsViewModel : BaseViewModel
    {

        private IDiscountDataService _discountDataService;
        private Notifier _notifier;
        private ConcurrentDictionary<string, string> _myDictionary;


        public SettingsViewModel(IDiscountDataService discountDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier)
        {
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
        }

 


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateMapRuleCommand))]
        private bool isEnableToMap;

        [ObservableProperty]
        private PremapExcelColumnsViewModel preMapRuleViewModel;


        [RelayCommand]
        public void CreateMapRule()
        {
            IsEnableToMap = true;
            this.PreMapRuleViewModel = new PremapExcelColumnsViewModel(_myDictionary, this);
        }

        [RelayCommand]
        public void Cancel()
        {
            if (IsEnableToMap)
                IsEnableToMap = false;
        }

    }
}
