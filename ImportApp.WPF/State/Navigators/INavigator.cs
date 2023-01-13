using CommunityToolkit.Mvvm.Input;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportApp.WPF.State.Navigators
{
    public enum ViewType
    {
        Home,
        Articles,
        ImportArticles,
        Settings,
        Discounts
    }


    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        //ICommand UpdateCurrentViewModelCommand { get; }
    }
}
