using CommunityToolkit.Mvvm.ComponentModel;
using ImportApp.Domain.Services;
using ImportApp.WPF.State.Store;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class StoreViewModel : BaseViewModel
    {
        private IArticleDataService _articleService;
        private Notifier _notifier;

        public IStore Store { get; set; }


        public StoreViewModel(IArticleDataService articleService, Notifier notifier, IStorageDataService storageDataService, ICategoryDataService categoryDataService)
        {
            _notifier = notifier;
            Store = new Store(articleService, _notifier, storageDataService, categoryDataService);
        }



    }
}