using ImportApp.Domain.Models;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static GenericDataService<Article> _dataService;


        public MainViewModel(GenericDataService<Article> dataService)
        {
            _dataService = dataService;
        }

        public INavigator Navigator { get; set; } = new Navigator(_dataService);

    }
}
