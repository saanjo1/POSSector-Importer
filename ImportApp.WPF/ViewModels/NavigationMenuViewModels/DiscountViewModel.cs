﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class DiscountViewModel : BaseViewModel
    {

        [ObservableProperty]
        private string excelFile;

        private readonly IExcelDataService _excelDataService;
        private readonly IArticleService _articleDataService;
        private readonly ICategoryService _categoryDataService;
        private readonly IRuleService _discountDataService;
        private readonly Notifier _notifier;
        private readonly ConcurrentDictionary<string, string> _myDictionary;

        [ObservableProperty]
        private ObservableCollection<MapColumnForDiscountViewModel> articlesCollection = new ObservableCollection<MapColumnForDiscountViewModel>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearAllDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(LoadFixedExcelColumnsCommand))]
        private int count;

        private string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                textToFilter = value;
                OnPropertyChanged(nameof(TextToFilter));
                ArticleCollection.Filter = FilterFunction;
            }
        }

        private bool FilterFunction(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var filt = obj as MapColumnForDiscountViewModel;
                return filt != null && (filt.BarCode.Contains(TextToFilter));
            }
            return true;
        }



        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        private bool isMapped;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        private bool isOptions;

        [ObservableProperty]
        private MapColumnForDiscountViewModel mapDataModel;

        [ObservableProperty]
        private DiscountOptionsViewModel discountOptionsModel;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearAllDataCommand))]
        ObservableCollection<MapColumnForDiscountViewModel>? articleList;

        [ObservableProperty]
        private ICollectionView articleCollection;

        private int currentPage = 1;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateEnableState();
            }
        }

        private int selectedRecord = 15;
        public int SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                UpdateRecordCount();
            }
        }

        private void UpdateRecordCount()
        {
            NumberOfPages = (int)Math.Ceiling((double)articleList.Count / SelectedRecord);
            NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;
            UpdateCollection(articleList.Take(SelectedRecord));
            CurrentPage = 1;
        }

        [ObservableProperty]
        private int numberOfPages = 15;

        [ObservableProperty]
        private bool isFirstEnabled;

        [ObservableProperty]
        private bool isPreviousEnabled;

        [ObservableProperty]
        private bool isNextEnabled;

        [ObservableProperty]
        private bool isLastEnabled;


        public static int RecordStartForm = 0;
        [RelayCommand]
        private void PreviousPage(object obj)
        {
            CurrentPage--;
            RecordStartForm = articleList.Count - SelectedRecord * (NumberOfPages - (CurrentPage - 1));

            var recordsToShow = articleList.Skip(RecordStartForm).Take(SelectedRecord);

            UpdateCollection(recordsToShow);
            UpdateEnableState();
        }

        [RelayCommand]
        private void LastPage(object obj)
        {
            var recordsToSkip = SelectedRecord * (NumberOfPages - 1);
            UpdateCollection(articleList.Skip(recordsToSkip));
            CurrentPage = NumberOfPages;
            UpdateEnableState();
        }


        [RelayCommand]
        private void FirstPage(object obj)
        {
            UpdateCollection(articleList.Take(SelectedRecord));
            CurrentPage = 1;
            UpdateEnableState();
        }

        [RelayCommand]
        private void NextPage(object obj)
        {
            RecordStartForm = CurrentPage * SelectedRecord;
            var recordsToShow = articleList.Skip(RecordStartForm).Take(SelectedRecord);
            UpdateCollection(recordsToShow);
            CurrentPage++;
            UpdateEnableState();
        }

        private void UpdateEnableState()
        {
            IsFirstEnabled = CurrentPage > 1;
            IsPreviousEnabled = CurrentPage > 1;
            IsNextEnabled = CurrentPage < NumberOfPages;
            IsLastEnabled = CurrentPage < NumberOfPages;
        }

        private void UpdateCollection(IEnumerable<MapColumnForDiscountViewModel> recordsToShow)
        {
            ArticlesCollection.Clear();
            foreach (var item in recordsToShow)
            {
                ArticlesCollection.Add(item);
            }
        }

        public DiscountViewModel(IExcelDataService excelDataService, Notifier notifier, ConcurrentDictionary<string, string> myDictionary, IArticleService articleDataService, ICategoryService categoryDataService, IRuleService discountDataService)
        {
            _excelDataService = excelDataService;
            _notifier = notifier;
            _myDictionary = myDictionary;
            _articleDataService = articleDataService;
            _categoryDataService = categoryDataService;
            _discountDataService = discountDataService;

        }

        [RelayCommand]
        public void LoadFixedExcelColumns()
        {
            MapColumnForDiscountViewModel tempVm = new MapColumnForDiscountViewModel();

            try
            {
                articleList = _excelDataService.ReadFromExcel(_myDictionary, tempVm).Result;
                if (articleList != null)
                {
                    _notifier.ShowInformation(articleList.Count + " articles pulled. ");
                    LoadData(articleList);
                }
                else
                {
                    _notifier.ShowError("Please check your ExcelFile & Sheet, and try again.");

                }
            }
            catch (Exception)
            {
                if (articleList == null)
                    _notifier.ShowError("Please check your ExcelFile & Sheet, and try again.");
                else
                    _notifier.ShowError(Translations.ErrorMessage);

            }

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
            this.DiscountOptionsModel = new DiscountOptionsViewModel(this, _notifier);
        }

        [RelayCommand]
        public void Close()
        {
            if (IsMapped)
                IsMapped = false;

            if (IsOptions)
                IsOptions = false;
        }


        [RelayCommand(CanExecute = nameof(CanClear))]
        public void ClearAllData()
        {
            if (articleList != null)
            {
                articleList.Clear();
                ArticleCollection = null;
                Count = 0;
            }
            else
            {
                _notifier.ShowError("Can not clear empty list.");
            }
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

            ArticleCollection = CollectionViewSource.GetDefaultView(ArticlesCollection);
            UpdateCollection(articlesCollection.Take(SelectedRecord));
            UpdateRecordCount();
            Count = ArticleList.Count;

        }


        [RelayCommand]
        public async void ImportItems()
        {
            IsLoading = true;

            await Task.Run(() =>
            {

                if (DiscountOptionsModel == null)
                {
                    _notifier.ShowWarning("Oops! You need to provide discount options. ");
                }
                else
                {
                    int importCounter = 0;
                    int updateCounter = 0;
                    int notImported = 0;
                    int counter = _articleDataService.GetArticlesCount().Result;

                    if (ArticleList != null)
                    {
                        Rule newRule;

                        try
                        {
                            for (int i = 0; i < articleList.Count; i++)
                            {
                                var articleID = _articleDataService.CompareArticlesByBarcode(articleList[i].BarCode).Result;
                                if (articleID != Guid.Empty)
                                {
                                    var article = _articleDataService.Get(articleID.ToString()).Result;

                                    Rule disc = _discountDataService.GetRuleByName(articleList[i].Discount).Result;
                                    if (disc != null && disc.Name == articleList[i].Discount && CheckDates(disc))
                                    {
                                        newRule = _discountDataService.Get(disc.Id.ToString()).Result;
                                    }
                                    else
                                    {
                                        newRule = new Rule()
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = articleList[i].Discount,
                                            ValidFrom = DiscountOptionsModel.ValidFrom,
                                            ValidTo = DiscountOptionsModel.ValidTo,
                                            Type = "Period",
                                            Active = DiscountOptionsModel.ActivateDiscount,
                                            IsExecuted = false
                                        };

                                        _discountDataService.Create(newRule);
                                    }

                                    Article newArticle = new Article()
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = articleList[i].Name,
                                        ArticleNumber = counter++,
                                        Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
                                        BarCode = articleList[i].BarCode,
                                        SubCategoryId = _categoryDataService.ManageSubcategories(articleList[i]?.Category, articleList[i]?.Storage).Result,
                                        Deleted = false,
                                        Order = counter++,
                                    };

                                    _articleDataService.Update(article.Id, newArticle);
                                    updateCounter++;


                                    RuleItem newRuleItem = new RuleItem()
                                    {
                                        Id = Guid.NewGuid(),
                                        ArticleId = article.Id,
                                        RuleId = newRule.Id,
                                        NewPrice = Helpers.Extensions.GetDecimal(articleList[i].NewPrice)
                                    };

                                    _discountDataService.CreateRuleItem(newRuleItem);
                                    importCounter++;
                                }
                                else
                                {
                                    notImported++;
                                }
                            }

                            if (importCounter > 0)
                            {
                                _notifier.ShowSuccess("Discounts for " + importCounter + " articles successfully added.");
                            }
                            else if (notImported > 0)
                            {
                                _notifier.ShowWarning("Discounts for " + notImported + " items can not be added. Discounts for non existing article is not possible.");

                            }
                        }
                        catch (Exception)
                        {

                            _notifier.ShowError("Something went wrong. Please try again.");
                        }
                    }
                }
            });

            IsLoading = false;
            ClearAllData();
        }

        private bool CheckDates(Rule disc)
        {
            TimeSpan diff = (disc.ValidFrom - DiscountOptionsModel.ValidFrom).Duration();
            TimeSpan diff1 = (disc.ValidTo - DiscountOptionsModel.ValidTo).Duration();

            double threshold = 5; // 5ms


            if (diff.TotalMilliseconds < threshold)
            {
                // Dates are the same
                return true;
            }

            return false;
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

            if (IsOptions)
                return false;

            if (Count < 0)
                return false;


            return true;

        }
    }
}
