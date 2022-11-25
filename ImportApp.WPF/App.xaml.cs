using ImportApp.Domain.Models;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImportApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private GenericDataService<Article> _dataService;

        public App()
        {
            _dataService = new GenericDataService<Article>(new EntityFramework.DBContext.ImportAppDbContextFactory());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new MainViewModel(_dataService);
            window.Show();

            base.OnStartup(e);
        }
    }
}
