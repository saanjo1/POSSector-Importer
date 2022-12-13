using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportApp.WPF.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {

        public INavigator Navigator { get; set; }

        public MainViewModel(IArticleDataService articleService, ICategoryDataService categoryService, IExcelDataService excelDataService)
        {

            Navigator = new Navigator(articleService, excelDataService, categoryService);

        }

        [RelayCommand]
        private void Close()
        {
            Application.Current.Shutdown();

        }

        [RelayCommand]
        private void Minimize(MainWindow window)
        {
            if(window != null)
            {
                window.WindowState = System.Windows.WindowState.Minimized;
            }
        }


        [RelayCommand]
        private void Maximize(MainWindow _window)
        {
            if (_window.WindowState == WindowState.Normal)
                _window.WindowState = WindowState.Maximized;
            else
                _window.WindowState = WindowState.Normal;
        }


    }
}
