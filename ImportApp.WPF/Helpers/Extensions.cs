using ImportApp.Domain.Models;
using ImportApp.WPF.Resources;
using ImportApp.WPF.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Printing;
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

        public static MapColumnForDiscountViewModel SelectedColumns(MapColumnForDiscountViewModel mColumnModel, List<string> columnNamesList, ConcurrentDictionary<string, string> _myDictionary)
        {
            for (int i = 0; i < columnNamesList.Count(); i++)
            {
                if (columnNamesList[i].Contains("Name"))
                    mColumnModel.Name = columnNamesList[i];

                if (columnNamesList[i].Contains("Barcode"))
                    mColumnModel.BarCode = columnNamesList[i];

                if (columnNamesList[i].Contains("Full price €"))
                    mColumnModel.Price = columnNamesList[i];

                if (columnNamesList[i].Contains("Category"))
                    mColumnModel.Category = columnNamesList[i];

                if (columnNamesList[i].Equals("Discount"))
                    mColumnModel.Discount = columnNamesList[i];

                if (columnNamesList[i].Contains("Discounted price"))
                    mColumnModel.NewPrice = columnNamesList[i];

                mColumnModel.Storage = "Articles";

            }

            return mColumnModel;  
        }


        public static string DisplayDiscountInPercentage(string discount)
        {
            bool success = double.TryParse(discount, out double result);
            string finalResult;

            if (success) 
            {
                var percentage = result * (-100);
                finalResult = percentage.ToString() + "%";    
            }
            else
            {
                finalResult = "NO DISCOUNT";
            }


            return finalResult;
        }



        public static string ReaderHelper(DbDataReader reader, string flag, ConcurrentDictionary<string, string> _myDictionary)
        {
            string value;
            string outputvalue = String.Empty;
            _myDictionary.TryGetValue(flag, out value);

            if (value != null)
            {
                string[] substrings = value.Split(';').Select(s => s.Trim()).ToArray();

                    foreach (string substring in substrings)
                    {
                        outputvalue += reader[substring] + " ";
                    }
            }

            return outputvalue;
        }
                
    }
}
