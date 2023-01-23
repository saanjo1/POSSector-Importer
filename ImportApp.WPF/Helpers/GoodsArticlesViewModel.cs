using CommunityToolkit.Mvvm.ComponentModel;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Models
{
    [ObservableObject]
    public partial class GoodsArticlesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Guid id;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private decimal? quantity;

        [ObservableProperty]
        private decimal? latestPrice;

        [ObservableProperty]
        private Guid storage;

        [ObservableProperty]
        private Guid goodId;

        [ObservableProperty]
        private decimal? price;


    }
}
