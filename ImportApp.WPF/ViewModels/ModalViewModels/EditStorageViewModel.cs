﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels;

[ObservableObject]
public partial class EditStorageViewModel
{

    private GoodsArticlesViewModel GoodsArticlesViewModel;
    private Notifier _notifier;
    private ICategoryService _categoryDataService;
    private IStorageService _storageDataService;
    private ArticleStorageViewModel viewModel;
    private InventoryDocument inventoryDocument;

    public EditStorageViewModel(GoodsArticlesViewModel goodsArticlesViewModel, Notifier notifier, ICategoryService categoryDataService, IStorageService storageDataService, ArticleStorageViewModel vm, InventoryDocument inventoryDocument)
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
        this.inventoryDocument = inventoryDocument;
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


            _notifier.ShowSuccess("Quantity updated!");
            viewModel.Cancel();
            viewModel.LoadData();

            viewModel.ListOfItems.Add(newInventoryItem);
        }

    }

    [RelayCommand]
    public void Cancel()
    {
        viewModel.Cancel();
    }

}

