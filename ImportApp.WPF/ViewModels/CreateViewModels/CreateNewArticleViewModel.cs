using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewArticleViewModel
    {
        private IArticleDataService articleDataService;
        private Notifier _notifier;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedSubCategory;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string name;

        [ObservableProperty]
        private Guid id;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private int articleNumber;


        [ObservableProperty]
        public HomeViewModel viewModel;


        [ObservableProperty]
        private int order;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string barcode;

        [ObservableProperty]
        private List<string> subCategory;

        public CreateNewArticleViewModel(IArticleDataService _articleDataService, HomeViewModel vm, Notifier notifier)
        {
            articleDataService = _articleDataService;
            ViewModel = vm;
            _notifier = notifier;
            LoadData();
        }


        public void LoadData()
        {
            try
            {
                Id = Guid.NewGuid();
                ArticleNumber = articleDataService.GetLastArticleNumber().Result;
                Order = 1;
                SubCategory = GetSubcategories().Result;
                if (SubCategory.Count > 0)
                {
                    SelectedSubCategory = SubCategory[0];
                }
                else
                {
                    _notifier.ShowWarning(Translations.NoSubcategories);
                }
            }
            catch (Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);
            }
        }


        [RelayCommand]
        private void Cancel()
        {
            viewModel.Close();
        }

        private Task<List<string>> GetSubcategories()
        {
            var subcategories = articleDataService.GetAllSubcategories().Result;
            List<string> result = new List<string>();

            foreach (var item in subcategories)
            {
                result.Add(item.Name);
            }

            return Task.FromResult(result);
        }



        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            if (articleDataService.GetArticleByName(Name).Result == Guid.Empty)
            {
                try
                {
                    Article newArticle = new Article
                    {
                        Id = Id,
                        ArticleNumber = ArticleNumber,
                        Price = Price,
                        BarCode = Barcode,
                        Order = Order,
                        Deleted = false,
                        ReturnFee = 0,
                        FreeModifiers = 0,
                        Code = null,
                        Tag = null,
                        Image = null,
                        Name = Name,
                        SubCategoryId = articleDataService.GetSubCategory(SelectedSubCategory).Result
                    };
                    articleDataService.Create(newArticle);
                    _notifier.ShowSuccess(Translations.CreatedArticle);
                    Cancel();
                }
                catch
                {
                    _notifier.ShowError(Translations.DuplicateArticle);
                    Cancel();
                }
            }

        }


        private bool CanSave()
        {
            if (Name != null && Barcode != null && SelectedSubCategory != null)
            {
                return true;
            }
            return false;
        }

    }
}
