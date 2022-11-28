using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        private IArticleService _articleService;

        public Navigator(IArticleService articleService)
        {
            _articleService = articleService;
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
                        break;
                    case ViewType.Articles:
                        this.CurrentViewModel = new ArticlesViewModel(_articleService);
                        break;
                    default:
                        break;
                }
            }
        }

       
    }
}
