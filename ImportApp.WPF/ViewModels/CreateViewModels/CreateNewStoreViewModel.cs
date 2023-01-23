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
    public partial class CreateNewStoreViewModel
    {

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? name;


        [ObservableProperty]
        private Guid id;

        private readonly HomeViewModel _homeViewModel;
        private Notifier _notifier;
        private IStorageDataService _storeDataService;

        public CreateNewStoreViewModel(HomeViewModel homeViewModel, Notifier notifier, IStorageDataService storeDataService)
        {
            _homeViewModel = homeViewModel;
            _notifier = notifier;
            _storeDataService = storeDataService;
            Id = Guid.NewGuid();
        }


        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            if (_storeDataService.GetStorageByName(Name) == null)
            {
                try
                {
                    Storage newStorage = new Storage()
                    {
                        Id = Id,
                        Name = Name,
                        Deleted = false
                    };
                    _storeDataService.Create(newStorage);
                    _notifier.ShowSuccess(Translations.CreatedStorage);
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
                _notifier.ShowError(Translations.DuplicateStorage);
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
