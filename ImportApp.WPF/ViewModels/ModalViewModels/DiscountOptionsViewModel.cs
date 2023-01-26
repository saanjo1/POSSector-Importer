using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class DiscountOptionsViewModel
    {
        [ObservableProperty]
        private DateTime validFrom;

        [ObservableProperty]
        private DateTime validTo; 
        
        [ObservableProperty]
        private bool activateDiscount;


        private DiscountViewModel _discountViewModel;
        private Notifier _notifier;
        public DiscountOptionsViewModel(DiscountViewModel discountViewModel, Notifier notifier)
        {
            _discountViewModel = discountViewModel;
            _notifier = notifier;

            if (ValidFrom == DateTime.MinValue)
                ValidFrom = DateTime.Now;
            if (ValidTo == DateTime.MinValue)
                ValidTo = DateTime.Now;
        }


        [RelayCommand]
        public void Save()
        {
                _discountViewModel.Close();
                _notifier.ShowSuccess("Options saved successfully.");
        }


        [RelayCommand]
        public void Cancel()
        {
            _discountViewModel.Close();
        }


    }
}
