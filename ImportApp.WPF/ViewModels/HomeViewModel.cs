using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel : BaseViewModel
    {

        private IArticleDataService _articleService;


        [ObservableProperty]
        private bool isOpen;

        [ObservableProperty]
        private CreateNewArticleViewModel addArticleModel;


        public HomeViewModel(IArticleDataService articleService)
        {
            _articleService = articleService;
        }

        [RelayCommand]
        private void AddArticle()
        {
            IsOpen = true;
            AddArticleModel = new CreateNewArticleViewModel(_articleService, this);
        }


        [RelayCommand]
        public void Close()
        {
            IsOpen = false;
        }

    }
}

