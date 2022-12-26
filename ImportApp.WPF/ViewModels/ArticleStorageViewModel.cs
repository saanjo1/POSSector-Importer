using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomMessageBox;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class ArticleStorageViewModel : BaseViewModel
    {
        private IArticleDataService _articleService;

        [ObservableProperty]
        private string storageName;


        private string count;

        public string Count
        {
            get { return articleList.Count + "articles found"; }
            set
            {
                count = value;
                OnPropertyChanged(nameof(TextToFilter));
            }
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

        private bool FilterFunction(object obj)
        {
            if (!string.IsNullOrEmpty(TextToFilter))
            {
                var filt = obj as Article;
                return filt != null && (filt.Name.Contains(TextToFilter) || filt.BarCode.Contains(TextToFilter) || filt.Price.ToString() == TextToFilter || filt.ArticleNumber.ToString() == TextToFilter);
            }
            return true;
        }



        public ArticleStorageViewModel(IArticleDataService articleService, string _storageName)
        {
            _articleService = articleService;
            storageName = _storageName;
            LoadData();
        }


        [ObservableProperty]
        private ICollection<Article> articleList;

        [ObservableProperty]
        private ObservableCollection<Article> articlesCollection = new ObservableCollection<Article>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteArticleCommand))]
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
        private void DeleteArticle(Article parameter)
        {
            try
            {
                var deletedArticle = _articleService.Delete(parameter.Id);
                bool? Result = new MessageBoxCustom("Article is successfully deleted.", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                LoadData();
            }
            catch (Exception)
            {
                bool? Result = new MessageBoxCustom("An error occured while attempting to delete article.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                throw;
            }
        }

        [RelayCommand]
        public void LoadData()
        {
            if(StorageName == "Articles")
            {
                ArticleList = _articleService.GetArticles().Result;
            }
            else
            {
                ArticleList = _articleService.GetEconomato().Result;
            }
            ArticleCollection = CollectionViewSource.GetDefaultView(ArticlesCollection);
            UpdateCollection(articlesCollection.Take(SelectedRecord));
            UpdateRecordCount();
        }


    }
}

