using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using System;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewDiscountViewModel
    {

        private readonly SettingsViewModel _settings;
        private Notifier _notifier;
        private IDiscountDataService _discountDataService;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? value;

        [ObservableProperty]
        private DateTime validFrom;

        [ObservableProperty]
        private DateTime validTo;

        [ObservableProperty]
        private bool active;


        public CreateNewDiscountViewModel(SettingsViewModel settings, IDiscountDataService discountDataService, Notifier notifier)
        {
            _settings = settings;
            _discountDataService = discountDataService;
            ValidFrom = DateTime.Now;
            ValidTo = DateTime.Now;
            _notifier = notifier;
        }

        [ObservableProperty]
        private bool isOpen;


        [RelayCommand]
        public void Cancel()
        {
            _settings.Cancel();
        }

        [RelayCommand]
        public void Save()
        {
            try
            {
                Rule newRule = new Rule()
                {
                    Id = Guid.NewGuid(),
                    Name = this.Name,
                    Type = this.Value,
                    ValidFrom = this.ValidFrom,
                    ValidTo = this.ValidTo,
                    Active = this.Active,
                    IsExecuted = true
                };
                _discountDataService.Create(newRule);
                _notifier.ShowSuccess("Rule successfully created.");
                Cancel();   
            }
            catch (Exception)
            {
                _notifier.ShowError("An error occured while creating discount.");
                Cancel();
            }

        }
    }
}
