using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ToastNotifications;
using System.Collections.Concurrent;
using ImportApp.WPF.Resources;
using ToastNotifications.Messages;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using ImportApp.Domain.Models;
using System;
using System.Diagnostics.Metrics;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class DiscountViewModel : BaseViewModel
    {
      
        [ObservableProperty]
        private string excelFile;

        private readonly IExcelDataService _excelDataService;
        private readonly IArticleDataService _articleDataService;
        private readonly ICategoryDataService _categoryDataService;
        private readonly IDiscountDataService _discountDataService;
        private readonly Notifier _notifier;
        private readonly ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearListCommand))]
        private int count;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OptionsCommand))]
        private bool isMapped;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OptionsCommand))]
        private bool isOptions;

        [ObservableProperty]
        private MapColumnForDiscountViewModel mapDataModel;

        [ObservableProperty]
        ObservableCollection<MapColumnForDiscountViewModel>? articleList;

        [ObservableProperty]
        private ICollectionView articleCollection;

        public DiscountViewModel(IExcelDataService excelDataService, Notifier notifier, ConcurrentDictionary<string, string> myDictionary, IArticleDataService articleDataService, ICategoryDataService categoryDataService, IDiscountDataService discountDataService)
        {
            _excelDataService = excelDataService;
            _notifier = notifier;
            _myDictionary = myDictionary;
            _articleDataService = articleDataService;
            _categoryDataService = categoryDataService;
            _discountDataService = discountDataService;
        }


        [RelayCommand(CanExecute = nameof(CanClick))]
        public void MapData()
        {
            this.IsMapped = true;
            this.MapDataModel = new MapColumnForDiscountViewModel(this, _excelDataService, _myDictionary, _notifier);
        }



        [RelayCommand(CanExecute = nameof(CanClickOptions))]
        public void Options()
        {
            this.IsOptions = true;
        }

        [RelayCommand]
        public void Close()
        {
            if (IsMapped)
                IsMapped = false;
        }


        [RelayCommand(CanExecute = nameof(CanClear))]
        public void ClearList()
        {
            articleList.Clear();
            Count = ArticleList.Count;
        }

        [RelayCommand]
        public void LoadData(ObservableCollection<MapColumnForDiscountViewModel>? vm)
        {
            if (vm != null)
            {
                articleList = vm;
                ArticleCollection = CollectionViewSource.GetDefaultView(vm);
            }
            IsMapped = false;

            Count = ArticleList.Count;

        }


        [RelayCommand]
        public void ImportItems()
        {
            int importCounter = 0;
            int updateCounter = 0;

            if (ArticleList.Count > 0 || ArticleList != null)
            {
                Rule newRule;

                for (int i = 0; i < articleList.Count; i++)
                {
                   Guid discId = _discountDataService.GetDiscountByName(articleList[i].Discount).Result;
                   if(discId == Guid.Empty)
                    {
                        newRule = new Rule()
                        {
                            Id = Guid.NewGuid(),
                            Name = articleList[i].Discount,
                            ValidFrom = DateTime.Now,
                            ValidTo = DateTime.Today.AddDays(1),
                            Type = "HappyHour",
                            Active = true,
                            IsExecuted = false
                        };

                        _discountDataService.Create(newRule);
                    }
                   else
                    {
                        newRule = _discountDataService.Get(discId.ToString()).Result;
                    }

                    Article newArticle = new Article()
                    {
                        Id = Guid.NewGuid(),
                        Name = articleList[i].Name,
                        ArticleNumber = articleList.Count + 1,
                        Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
                        BarCode = articleList[i].BarCode,
                        SubCategoryId = _categoryDataService.ManageSubcategories(articleList[i]?.Category, articleList[i]?.Storage).Result,
                        Deleted = false,
                        Order = 1,
                    };

                    string? value = articleList[i].BarCode;
                    Article temp = _articleDataService.Compare(value).Result;

                    if (temp == null)
                    {
                        _articleDataService.Create(newArticle);
                        importCounter++;

                        RuleItem newRuleItem = new RuleItem()
                        {
                            Id = Guid.NewGuid(),
                            ArticleId = newArticle.Id,
                            RuleId = newRule.Id,
                            NewPrice = Helpers.Extensions.GetDecimal(articleList[i].NewPrice)
                        };

                        _discountDataService.CreateDiscountItem(newRuleItem);

                    }
                    else
                    {
                        _articleDataService.Update(temp.Id, newArticle); 
                        updateCounter++;


                        RuleItem newRuleItem = new RuleItem()
                        {
                            Id = Guid.NewGuid(),
                            ArticleId = temp.Id,
                            RuleId = newRule.Id,
                            NewPrice = Helpers.Extensions.GetDecimal(articleList[i].NewPrice)
                        };

                        _discountDataService.CreateDiscountItem(newRuleItem);
                    }
                }

                _notifier.ShowSuccess(Translations.SuccessImport);
                _notifier.ShowInformation(updateCounter + " items updated." );
                _notifier.ShowInformation(importCounter + " items imported." );
                ClearList();
            }
            else
            {
                _notifier.ShowError(Translations.ErrorMessage);
            }
        }


        public bool CanClick()
        {
            if (_myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value) == false)
            {
                _notifier.ShowWarning(Translations.SettingsError);
                return false;
            }
            if (IsMapped)
                return false;
            if (IsOptions)
                return false;

            if (articleList != null)
                return false;
           
                return true;
        }


        public bool CanClear()
        {
            if (Count > 0)
                return true;
            return false;
        }
        
        public bool CanClickOptions()
        {
            if (_myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value) == false)
            {
                _notifier.ShowWarning(Translations.SettingsError);
                return false;
            }
            if (IsMapped)
                return false;
            if (IsOptions)
                return false;

            if (articleList == null)
                return false;


            return true;

        }
    }
}
