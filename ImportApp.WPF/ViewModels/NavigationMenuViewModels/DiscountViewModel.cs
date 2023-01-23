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
using System.Linq;
using System.Collections.Generic;

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
        private ObservableCollection<MapColumnForDiscountViewModel> articlesCollection = new ObservableCollection<MapColumnForDiscountViewModel>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearAllDataCommand))]
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
        [NotifyCanExecuteChangedFor(nameof(OptionsCommand))]
        private bool isMapped;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MapDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(OptionsCommand))]
        private bool isOptions;

        [ObservableProperty]
        private MapColumnForDiscountViewModel mapDataModel;

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

        public DiscountViewModel(IExcelDataService excelDataService, Notifier notifier, ConcurrentDictionary<string, string> myDictionary, IArticleDataService articleDataService, ICategoryDataService categoryDataService, IDiscountDataService discountDataService)
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
                if(articleList != null)
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
        }

        [RelayCommand]
        public void Close()
        {
            if (IsMapped)
                IsMapped = false;
        }


        [RelayCommand(CanExecute = nameof(CanClear))]
        public void ClearAllData()
        {
            if (articleList != null)
            {
                articleList.Clear();
                ArticleCollection = null;
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
        public void ImportItems()
        {
            int importCounter = 0;
            int updateCounter = 0;
            int counter = _articleDataService.GetLastArticleNumber().Result;

            if (ArticleList != null)
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
                        ArticleNumber = counter++,
                        Price = Helpers.Extensions.GetDecimal(articleList[i].Price),
                        BarCode = articleList[i].BarCode,
                        SubCategoryId = _categoryDataService.ManageSubcategories(articleList[i]?.Category, articleList[i]?.Storage).Result,
                        Deleted = false,
                        Order = counter++,
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
                ClearAllData();
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
