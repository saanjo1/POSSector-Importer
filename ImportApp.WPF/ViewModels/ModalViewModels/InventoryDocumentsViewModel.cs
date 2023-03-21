using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels.ModalViewModels
{
    [ObservableObject]
    public partial class InventoryDocumentsViewModel
    {
        [ObservableProperty]
        private string dateTime;

        [ObservableProperty]
        private string supplier;

        [ObservableProperty]
        private decimal? totalInputPrice;

        [ObservableProperty]
        private string totalSoldPrice;

        [ObservableProperty]
        private string totalIncome;

        [ObservableProperty]
        private string totalTaxes;  
        
        [ObservableProperty]
        private decimal pricewithoutTaxes;


        public InventoryDocumentsViewModel()
        {
            
        }

    }
}
