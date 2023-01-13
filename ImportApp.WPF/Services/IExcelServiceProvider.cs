﻿using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.Services
{
    public interface IExcelServiceProvider<T>
    {
        Task<string> OpenDialog();
        Task<List<string>> ListSheetsFromFile(string excelName);
        Task<List<string>> ListColumnNames(string excelName);
        Task<ObservableCollection<T>> ReadFromExcel(ConcurrentDictionary<string,string> model, MapColumnViewModel viewModel);
        Task<ObservableCollection<ArticleQtycViewModel>> ReadFromExcel(ConcurrentDictionary<string,string> model, ArticleQtycViewModel viewModel);
    }
}
