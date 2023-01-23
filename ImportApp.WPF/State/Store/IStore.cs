using ImportApp.WPF.ViewModels;

namespace ImportApp.WPF.State.Store
{
    public enum StoreType
    {
        Articles,
        Economato
    }


    public interface IStore
    {
        BaseViewModel? CurrentDataGrid { get; set; }
        //ICommand UpdateCurrentViewModelCommand { get; }
    }
}
