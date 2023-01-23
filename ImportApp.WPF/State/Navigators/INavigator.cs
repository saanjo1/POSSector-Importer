using ImportApp.WPF.ViewModels;

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
