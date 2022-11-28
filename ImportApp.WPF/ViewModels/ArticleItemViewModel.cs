using CommunityToolkit.Mvvm.ComponentModel;
using ImportApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    public partial class ArticleItemViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string ArticleNumber { get; set; } 
        public string Barcode { get; set; } 
        public string Gender { get; set; } 
        public string Quantity { get; set; } 
        public string Price { get; set; } 
    }
}
