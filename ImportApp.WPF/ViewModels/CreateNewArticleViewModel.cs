using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class CreateNewArticleViewModel
    {
        private IArticleDataService articleDataService;


        [ObservableProperty]
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
        private string barcode;

        [ObservableProperty]
        private List<SubCategory> subCategory;

        public CreateNewArticleViewModel(IArticleDataService _articleDataService, HomeViewModel vm)
        {
            articleDataService = _articleDataService;
            ViewModel = vm;
            LoadData();
        }


        public void LoadData()
        {
            Id = Guid.NewGuid();
            ArticleNumber = articleDataService.GetLastArticleNumber().Result;
            Order = 1;
        }


        [RelayCommand]
        private void Cancel()
        {
            viewModel.Close();
        }
    }
}
