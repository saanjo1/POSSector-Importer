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
    public partial class CreateCategoryViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? name;


        [ObservableProperty]
        private Guid id;


        private readonly HomeViewModel _homeViewModel;
        private Notifier _notifier;
        private ICategoryDataService _categoryDataService;

        public CreateCategoryViewModel(HomeViewModel homeViewModel, Notifier notifier, ICategoryDataService categoryDataService)
        {
            _homeViewModel = homeViewModel;
            _notifier = notifier;
            _categoryDataService = categoryDataService;
            Id = Guid.NewGuid();
        }


        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            try
            {
                Category newCategory = new Category()
                {
                    Id = Id,
                    Name = Name,
                    Deleted = false
                };
                _categoryDataService.Create(newCategory);
                _notifier.ShowSuccess(Translations.CreatedCategory);
                Cancel();
            }
            catch (Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);
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
