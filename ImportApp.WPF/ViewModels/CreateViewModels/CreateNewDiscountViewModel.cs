using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewDiscountViewModel
    {

        private readonly HomeViewModel _homeViewModel;
        private Notifier _notifier;
        private IDiscountDataService _discountDataService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private Guid id;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? value;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private DateTime validFrom;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private DateTime validTo;

        [ObservableProperty]
        private bool active;


        public CreateNewDiscountViewModel(HomeViewModel homeViewModel, IDiscountDataService discountDataService, Notifier notifier)
        {
            _homeViewModel = homeViewModel;
            _discountDataService = discountDataService;
            ValidFrom = DateTime.Now;
            ValidTo = DateTime.Now;
            _notifier = notifier;
            Id = Guid.NewGuid();
        }

        [ObservableProperty]
        private bool isOpen;


        [RelayCommand]
        public void Cancel()
        {
            _homeViewModel.Close();
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            try
            {
                Rule newRule = new Rule()
                {
                    Id = Id,
                    Name = this.Name,
                    Type = this.Value,
                    ValidFrom = this.ValidFrom,
                    ValidTo = this.ValidTo,
                    Active = this.Active,
                    IsExecuted = true
                };
                _discountDataService.Create(newRule);
                _notifier.ShowSuccess(Translations.CreatedDiscount);
                Cancel();
            }
            catch (Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);
                Cancel();
            }

        }


        public bool CanSave()
        {
            if (Name != null && ValidFrom != null && ValidTo != null && Value != null)
                return true;
            return false;
        }
    }
}
