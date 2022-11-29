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
        public static IArticleService _articleService = new IArticleService();  
        public static IExcelDataService _excelDataService = new IExcelDataService();  

        public MainViewModel()
        {
            
        }

        public INavigator Navigator { get; set; } = new Navigator(_articleService, _excelDataService);

    }
}
