using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateSupplierViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? name;

        [ObservableProperty]
        private string? address;

        [ObservableProperty]
        private string? vatNumber;


        [ObservableProperty]
        private Guid id;


        private readonly HomeViewModel _homeViewModel;
        private Notifier _notifier;
        private ISupplierDataService _supplierDataService;

        public CreateSupplierViewModel(HomeViewModel homeViewModel, Notifier notifier, ISupplierDataService supplierDataService)
        {
            _homeViewModel = homeViewModel;
            _notifier = notifier;
            _supplierDataService = supplierDataService;
            Id = Guid.NewGuid();
        }


        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            if (_supplierDataService.GetSupplierByName(Name) == null)
            {
                try
                {
                    Supplier newSupplier = new Supplier()
                    {
                        Id = Id,
                        Name = Name,
                        IsDeleted = false,
                        Address = Address,
                        VatNumber = VatNumber
                    };
                    _supplierDataService.Create(newSupplier);
                    _notifier.ShowSuccess(Translations.CreatedSupplier);
                    Cancel();
                }
                catch (Exception)
                {
                    _notifier.ShowError(Translations.ErrorMessage);
                    Cancel();
                }
            }
            else
            {
                _notifier.ShowError(Translations.DuplicatedSupplier);
                Cancel();
            }
        }


        [RelayCommand]
        public void Cancel()
        {
            _homeViewModel.Close();
        }

        private bool CanSave()
        {
            if (Name != null)
                return true;
            return false;
        }
    }
}
