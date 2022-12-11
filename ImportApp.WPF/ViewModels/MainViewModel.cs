using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
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
        public static IArticleService _articleService = new IArticleService();  
        public static IExcelDataService _excelDataService = new IExcelDataService();
        public INavigator Navigator { get; set; } = new Navigator(_articleService, _excelDataService);


        public MainViewModel()
        {
            
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
