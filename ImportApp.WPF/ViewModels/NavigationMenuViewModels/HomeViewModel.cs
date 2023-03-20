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
using System.Windows.Data;
using ToastNotifications;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierDataService;


        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private ObservableCollection<InventoryDocumentsViewModel> listOfInventories;

        [ObservableProperty]
        private ICollectionView inventoryCollection;


        public HomeViewModel(ICategoryService categoryService, ISupplierService supplierDataService)
        {
            _categoryService = categoryService;
            _supplierDataService = supplierDataService;
            Title = Translations.InventoryDocuments;
            LoadInventoryDocuments();
        }

        private void LoadInventoryDocuments()
        {
            var inventoryDocuments = _categoryService.GetInventoryDocuments().Result;

            ListOfInventories = new ObservableCollection<InventoryDocumentsViewModel>();

            foreach (var inventoryDocument in inventoryDocuments.Where(x=>x.SupplierId !=null))
            {
                ListOfInventories.Add(new InventoryDocumentsViewModel
                {
                    DateTime = inventoryDocument.Created.ToString("dd.MM.yyyy hh:mm"),
                    TotalInputPrice = GetTotalIncome(inventoryDocument),
                    Supplier = _supplierDataService.Get(inventoryDocument.SupplierId.ToString()).Result.Name != null ? _supplierDataService.Get(inventoryDocument.SupplierId.ToString()).Result.Name : "- - -"
                });
            }


            inventoryCollection = CollectionViewSource.GetDefaultView(ListOfInventories);
        }


        public decimal? GetTotalIncome(InventoryDocument inventoryDocument)
        {
            return _categoryService.GetTotalInventoryItems(inventoryDocument.Id.ToString()).Result;
        }
    }
}

