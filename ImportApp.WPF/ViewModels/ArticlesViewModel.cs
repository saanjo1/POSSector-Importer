using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class ArticlesViewModel : BaseViewModel
    {
        private IArticleDataService _articleService;
        public string Count { get { return articleList.Count + " articles found"; } }

        public ArticlesViewModel(IArticleDataService articleService)
        {
            _articleService = articleService;
            LoadData();
        }

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


        [ObservableProperty]
        private ICollection<Article> articleList;

        [ObservableProperty]
        private ObservableCollection<Article> articlesCollection = new ObservableCollection<Article>();

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

        private void UpdateCollection(IEnumerable<Article> recordsToShow)
        {
            ArticlesCollection.Clear();
            foreach (var item in recordsToShow)
            {
                ArticlesCollection.Add(item);
            }
        }


        [RelayCommand]
        public void SwitchTab()
        {

        }



        [RelayCommand]
        public void LoadData()
        {
            ArticleList = _articleService.GetAll().Result;
            ArticleCollection = CollectionViewSource.GetDefaultView(ArticlesCollection);
            UpdateCollection(articlesCollection.Take(SelectedRecord));
            UpdateRecordCount();
        }

        private bool FilterFunction(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var filt = obj as Article;
                return filt != null && (filt.Name.Contains(TextToFilter) || filt.BarCode.Contains(TextToFilter) || filt.Price.ToString() == TextToFilter || filt.ArticleNumber.ToString() == TextToFilter);
            }
            return true;
        }

    }
}