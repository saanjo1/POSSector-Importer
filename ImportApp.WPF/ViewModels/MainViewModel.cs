using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using System;
using System.Collections.Concurrent;
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

        public ConcurrentDictionary<string, string> myDictionary;

        public MainViewModel(IArticleDataService articleService, ICategoryDataService categoryService, IExcelDataService excelDataService, IDiscountDataService discountDataService)
        {
            myDictionary = new ConcurrentDictionary<string, string>();
            LoadDictionary();
            Navigator = new Navigator(articleService, excelDataService, categoryService, discountDataService, myDictionary);

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

        public void LoadDictionary()
        {
            myDictionary.TryAdd("Name", "Name");
        }

    }
}
