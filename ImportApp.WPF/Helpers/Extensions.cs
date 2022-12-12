using ImportApp.Domain.Models;
using ImportApp.WPF.Resources;
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


    }
}
