using CommunityToolkit.Mvvm.ComponentModel;
using ImportApp.Domain.Models;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private GenericDataService<Article> _dataService;

        private ICollection<Article> Articles;

        public ArticlesViewModel(GenericDataService<Article> dataService)
        {
            _dataService = dataService;
        }

    }
}
