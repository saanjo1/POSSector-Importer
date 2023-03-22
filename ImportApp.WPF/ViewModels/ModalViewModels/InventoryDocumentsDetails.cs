using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels.ModalViewModels
{
    [ObservableObject]
    public partial class InventoryDocumentsDetails
    {
        public InventoryDocumentsViewModel _viewmodel;
        public InventoryDocumentsDetails(InventoryDocumentsViewModel viewModel)
        {
           _viewmodel = viewModel;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private decimal avgpricePerUnit;

        [ObservableProperty]
        private decimal quantityIn;




        [ObservableProperty]
        private decimal taxAmount;

        [ObservableProperty]
        private decimal basePrice;

        [ObservableProperty]
        private decimal total;

    }
}
