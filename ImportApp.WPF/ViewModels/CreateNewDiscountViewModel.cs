using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomMessageBox;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using System;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewDiscountViewModel
    {

        private readonly SettingsViewModel _settings;
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


        public CreateNewDiscountViewModel(SettingsViewModel settings, IDiscountDataService discountDataService)
        {
            _settings = settings;
            _discountDataService = discountDataService;
            ValidFrom = DateTime.Now;
            ValidTo = DateTime.Now;
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
                bool? Result = new MessageBoxCustom("Successfully created discount.", MessageType.Success, MessageButtons.Ok).ShowDialog();
                Cancel();   
            }
            catch (Exception)
            {
                bool? Result = new MessageBoxCustom("An error occured while creating discount.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                Cancel();
            }

        }
    }
}
