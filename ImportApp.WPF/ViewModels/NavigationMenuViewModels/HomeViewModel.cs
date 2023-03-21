using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using ImportApp.WPF.ViewModels.ModalViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierDataService;
        private readonly IArticleService _articleService;


        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isShowDetailsOpen;

        [ObservableProperty]
        private ObservableCollection<InventoryDocumentsViewModel> listOfInventories;

        [ObservableProperty]
        private InventoryDocumentsDetails inventoryDocumentDetails;


        [ObservableProperty]
        private ICollectionView inventoryCollection;


        public HomeViewModel(ICategoryService categoryService, ISupplierService supplierDataService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _supplierDataService = supplierDataService;
            _articleService = articleService;
            Title = Translations.InventoryDocuments;
            LoadInventoryDocuments();
        }

        private async void LoadInventoryDocuments()
        {
            IsLoading = true;

            await Task.Run(() =>
            {

                var inventoryDocuments = _categoryService.GetInventoryDocuments().Result;

                ListOfInventories = new ObservableCollection<InventoryDocumentsViewModel>();

                foreach (var inventoryDocument in inventoryDocuments.Where(x => x.SupplierId != null))
                {
                    ListOfInventories.Add(new InventoryDocumentsViewModel
                    {
                        DateTime = inventoryDocument.Created.ToString("dd.MM.yyyy hh:mm"),
                        TotalInputPrice = GetTotalIncome(inventoryDocument),
                        Supplier = _supplierDataService.Get(inventoryDocument.SupplierId.ToString()).Result.Name != null ? _supplierDataService.Get(inventoryDocument.SupplierId.ToString()).Result.Name : "- - -",
                        TotalSoldPrice = _articleService.GetTotalSellingPrice(inventoryDocument).Result
                    });
                }

                InventoryCollection = CollectionViewSource.GetDefaultView(ListOfInventories);
            });

            IsLoading = false;
        }

        [RelayCommand]
        public void ShowInventoryDetails(InventoryDocumentsViewModel parameter)
        {
            IsShowDetailsOpen = true;
        }


        public decimal? GetTotalIncome(InventoryDocument inventoryDocument)
        {
            return _categoryService.GetTotalInventoryItems(inventoryDocument.Id.ToString()).Result;
        }
    }
}

