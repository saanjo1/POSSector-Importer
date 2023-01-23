using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels;

[ObservableObject]
public partial class EditStorageViewModel
{

    private GoodsArticlesViewModel GoodsArticlesViewModel;
    private Notifier _notifier;
    private ICategoryDataService _categoryDataService;
    private IStorageDataService _storageDataService;
    private ArticleStorageViewModel viewModel;

    public EditStorageViewModel(GoodsArticlesViewModel goodsArticlesViewModel, Notifier notifier, ICategoryDataService categoryDataService, IStorageDataService storageDataService, ArticleStorageViewModel vm)
    {
        GoodsArticlesViewModel = goodsArticlesViewModel;
        viewModel = vm;
        Name = GoodsArticlesViewModel.Name;
        Quantity = GoodsArticlesViewModel.Quantity;
        CurrentQuantity = GoodsArticlesViewModel.Quantity;
        LatestPrice = GoodsArticlesViewModel.LatestPrice;
        GoodId = GoodsArticlesViewModel.GoodId;
        Storage = GoodsArticlesViewModel.Storage;
        _notifier = notifier;
        _categoryDataService = categoryDataService;
        _storageDataService = storageDataService;
    }


    [ObservableProperty]
    private decimal? quantity;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private decimal? currentQuantity;

    [ObservableProperty]
    private Guid storage;

    [ObservableProperty]
    private Guid goodId;

    [ObservableProperty]
    private decimal? latestPrice;



    [RelayCommand]
    public void Save()
    {
        if (Quantity == CurrentQuantity)
        {
            _notifier.ShowInformation("No changes applied.");
            viewModel.Cancel();
        }
        else
        {
            InventoryDocument inventoryDocument = new InventoryDocument
            {
                Created = DateTime.Now,
                Order = _categoryDataService.GetInventoryCounter().Result,
                Id = Guid.NewGuid(),
                StorageId = (Guid?)storage,
                SupplierId = null,
                Type = 2,
                IsActivated = true,
                IsDeleted = false
            };

            _categoryDataService.CreateInventoryDocument(inventoryDocument);

            InventoryItemBasis newInventoryItem = new InventoryItemBasis()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Quantity = (decimal)Quantity - (decimal)CurrentQuantity,
                IsDeleted = false,
                Price = LatestPrice,
                Tax = 0,
                Total = ((decimal)Quantity - (decimal)CurrentQuantity) * LatestPrice,
                Discriminator = "InventoryDocumentItem",
                InventoryDocumentId = inventoryDocument.Id,
                StorageId = inventoryDocument.StorageId,
                GoodId = GoodId,
                CurrentQuantity = (decimal)Quantity - (decimal)CurrentQuantity,
            };

            _categoryDataService.CreateInventoryItem(newInventoryItem);
            _notifier.ShowSuccess("You just updated your storage!");
            viewModel.Cancel();
            viewModel.LoadData();

        }

    }

    [RelayCommand]
    public void Cancel()
    {
        viewModel.Cancel();
    }

}

