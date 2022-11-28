using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class ArticlesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ICollection<Article> articleList;


        private IArticleService _articleService;

        [RelayCommand]
        public void LoadData()
        {
            ArticleList = _articleService.GetAll().Result;
        }

        public ArticlesViewModel(IArticleService articleService)
        {
            _articleService = articleService;
            LoadData();
        }

    }
}