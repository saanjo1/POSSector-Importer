using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
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
    public partial class CreateNewSubcategoryViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string? name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedStorage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedCategory;

        [ObservableProperty]
        private Guid id;

        [ObservableProperty]
        private List<string> categories;

        [ObservableProperty]
        private List<string> storages;


        private readonly HomeViewModel _homeViewModel;
        private Notifier _notifier;
        private ISubcategoryDataService _subcategoryDataService;
        private ICategoryDataService _categoryDataService;
        private IStorageDataService _storeDataService;

        public CreateNewSubcategoryViewModel(HomeViewModel homeViewModel, Notifier notifier, ISubcategoryDataService subcategoryDataService, ICategoryDataService categoryDataService, IStorageDataService storeDataService)
        {
            _homeViewModel = homeViewModel;
            _notifier = notifier;
            _subcategoryDataService = subcategoryDataService;
            _categoryDataService = categoryDataService;
            _storeDataService = storeDataService;
            LoadData();
        }


        private void LoadData()
        {
            Id = Guid.NewGuid();
            storages = _storeDataService.GetNamesOfStorages().Result;
            categories = _categoryDataService.GetNamesOfCategories().Result;

            if (storages.Count > 0)
                SelectedStorage = storages[0];
            else
                _notifier.ShowWarning(Translations.NoStorages);

            if (categories.Count > 0)
                SelectedCategory = categories[0];
            else
                _notifier.ShowWarning(Translations.NoCategories);

        }


        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            if(_subcategoryDataService.GetSubCategoryByName(Name) == null)
            {
                try
                {
                    SubCategory newSubCategory = new SubCategory()
                    {
                        Id = Id,
                        Name = Name,
                        Deleted = false,
                        Tag = null,
                        CategoryId = _categoryDataService.GetCategoryByName(SelectedCategory).Result,
                        StorageId = _storeDataService.GetStorageByName(SelectedStorage).Result
                    };
                    _subcategoryDataService.Create(newSubCategory);
                    _notifier.ShowSuccess(Translations.CreatedSubCategory);
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
                _notifier.ShowError(Translations.DuplicateSubcategory);
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