using CommunityToolkit.Mvvm.ComponentModel;
using ImportApp.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapColumnViewModel : BaseViewModel
    {
        public static string SheetName;


        public static IExcelDataService _excelDataService;

        [ObservableProperty]
        private List<string> columnNamesList;


        public MapColumnViewModel(IExcelDataService excelDataService, string sheetName, List<string> _columnNames)
        {
            _excelDataService = excelDataService;
            columnNamesList = _columnNames;
            SheetName = sheetName;
        }


    }
}
