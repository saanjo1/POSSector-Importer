using ImportApp.Domain.Models;
using ImportApp.WPF.Resources;
using ImportApp.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.Helpers
{
    public class Extensions
    {
        public static OpenFileDialog CreateOFDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:";
            openFileDialog.Title = Translations.OpenDialogTitle;
            openFileDialog.Filter = Translations.OpenDialogFilter;

            return openFileDialog;
        }


        public static string SetOleDbConnection(string excelfile)
        {
            string con =
       @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelfile + ";" +
       @"Extended Properties='Excel 12.0;HDR=No;IMEX=1'";
            return con;
        }


        public static decimal GetDecimal(string value)
        {
            decimal decimalValue;
            if (value == "" || value == null)
                decimalValue = decimal.Parse("0");
            else
                decimalValue = decimal.Parse(value);

            return decimalValue;
        }

        public static MapColumnViewModel SelectedColumns(MapColumnViewModel mColumnModel, List<string> columnNamesList)
        {
            for (int i = 0; i < columnNamesList.Count(); i++)
            {
                if (columnNamesList[i].Contains("SKU"))
                    mColumnModel.Name = columnNamesList[i];

                if (columnNamesList[i].Contains("BARCODE"))
                    mColumnModel.BarCode = columnNamesList[i];

                if (columnNamesList[i].Contains("PRICE"))
                    mColumnModel.Price = columnNamesList[i];

                if (columnNamesList[i].Contains("STORAGE"))
                    mColumnModel.Storage = columnNamesList[i];

                if (columnNamesList[i].Contains("SEASON"))
                    mColumnModel.Collection = columnNamesList[i];

                if (columnNamesList[i].Contains("GENDER"))
                    mColumnModel.Gender = columnNamesList[i];
            }

            return mColumnModel;  
        }

    }
}
