using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportApp.WPF.State.Navigators
{
    public partial class Navigator : ObservableObject, INavigator
    {

        [ObservableProperty]
        private BaseViewModel? _currentViewModel;

        [ObservableProperty]
        private string caption;

        [ObservableProperty]
        private IconChar icon;


        private IArticleService _articleService;
        private IExcelDataService _excelDataService;

        public Navigator(IArticleService articleService, IExcelDataService excelDataService)
        {
            _articleService = articleService;
            _excelDataService = excelDataService;   
        }

        [RelayCommand]
        public void EditCurrentViewModel(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        this.CurrentViewModel = new HomeViewModel();
                        Caption = "Dashboard";
                        Icon = IconChar.Home;
                        break;
                    case ViewType.Articles:
                        this.CurrentViewModel = new ArticlesViewModel(_articleService);
                        Caption = "Articles";
                        Icon = IconChar.TableList;
                        break;
                    case ViewType.ImportArticles:
                        this.CurrentViewModel = new ImportArticleViewModel(_excelDataService);
                        Caption = "Import";
                        Icon = IconChar.FileExcel;
                        break;
                    default:
                        break;
                }
            }
        }

       
    }
}
