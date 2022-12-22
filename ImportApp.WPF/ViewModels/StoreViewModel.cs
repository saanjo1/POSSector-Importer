using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using ImportApp.WPF.State.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ImportApp.WPF.ViewModels
{

    [ObservableObject]
    public partial class StoreViewModel : BaseViewModel
    {
        private IArticleDataService _articleService;

        public IStore Store { get; set; }


        public StoreViewModel(IArticleDataService articleService)
        {
            _articleService = articleService;
            Store = new Store(_articleService);
        }



    }
}