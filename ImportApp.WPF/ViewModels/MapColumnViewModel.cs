using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string articleNumber;

        [ObservableProperty]
        private string price;

        [ObservableProperty]
        private string barCode;

        [ObservableProperty]
        private string order;






    }
}
