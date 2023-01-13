using ImportApp.Domain.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading.Tasks;
using ImportApp.WPF.ViewModels;
using System.Collections.ObjectModel;
using ImportApp.WPF;
using System.Collections.Concurrent;
using ImportApp.WPF.Resources;
using CommunityToolkit.Mvvm.Input;

namespace ImportApp.WPF.Services
{
    public class ExcelDataService : IExcelDataService
    {

        public static string ExcelFile { get; set; }

        public static OleDbConnection _oleDbConnection;
        public static OleDbCommand Command;

        private static ObservableCollection<MapColumnViewModel> mapColumnViewModels = new ObservableCollection<MapColumnViewModel>();
        private static ObservableCollection<ArticleQtycViewModel> _articleQtycViewModels = new ObservableCollection<ArticleQtycViewModel>();

        public ExcelDataService()
        {
        }


        public async Task<string> OpenDialog()
        {
            OpenFileDialog openFIleDialog = WPF.Helpers.Extensions.CreateOFDialog();

            if (openFIleDialog.ShowDialog() == true)
            {
                ExcelFile = openFIleDialog.FileName;
                return await Task.FromResult(ExcelFile);
            }

            return null;
        }


        public async Task<List<string>> ListSheetsFromFile(string excelFile)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = excelFile;
            if (Path.GetExtension(ExcelFile).Equals(".xls"))//for 97-03 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            }
            else if (Path.GetExtension(ExcelFile).Equals(".xlsx"))  //for 2007 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            }
            sbConnection.Add("Extended Properties", strExtendedProperties);

            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                await conn.OpenAsync();
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                    {
                        listSheet.Add(drSheet["TABLE_NAME"].ToString());
                    }
                }
            }
            return await Task.FromResult(listSheet);
        }

        public async Task<List<string>> ListColumnNames(string sheetName)
        {
            var connection = WPF.Helpers.Extensions.SetOleDbConnection(ExcelFile);

            _oleDbConnection = new OleDbConnection(connection);

            var lines = new List<string>();

            await _oleDbConnection.OpenAsync();

            Command = new OleDbCommand();
            Command.Connection = _oleDbConnection;
            Command.CommandText = "select top 1 * from [" + sheetName + "]";

            var Reader = await Command.ExecuteReaderAsync();


            while (Reader.Read())
            {
                var fieldCount = Reader.FieldCount;

                var fieldIncrementor = 1;
                var fields = new List<string>();
                while (fieldCount >= fieldIncrementor)
                {
                    string test = Reader[fieldIncrementor - 1].ToString();
                    fields.Add(test);
                    fieldIncrementor++;
                }

                lines = fields;
            }

            Reader.Close();
            _oleDbConnection.Close();


            return await Task.FromResult(lines);
        }

        public async Task<ObservableCollection<MapColumnViewModel>> ReadFromExcel(ConcurrentDictionary<string, string> _myDictionary, MapColumnViewModel viewModel)
        {
            bool success = _myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value);
            bool sheet = _myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string sheetValue);

            if (success)
            {
                try
                {
                    string _connection =
           @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + value + ";" +
           @"Extended Properties='Excel 8.0;HDR=Yes;'";

                    _oleDbConnection = new OleDbConnection(_connection);

                    await _oleDbConnection.OpenAsync();

                    Command = new OleDbCommand();
                    Command.Connection = _oleDbConnection;
                    Command.CommandText = "select * from [" + sheetValue + "]";

                    System.Data.Common.DbDataReader Reader = await Command.ExecuteReaderAsync();

                    while (Reader.Read())
                    {

                        mapColumnViewModels.Add(new MapColumnViewModel
                        {
                            Name = Reader[viewModel.Name].ToString(),
                            SubCategory = Reader[viewModel.SubCategory].ToString(),
                            Category = Reader[viewModel.Category].ToString(),
                            Storage = Reader[viewModel.Storage].ToString(),
                            BarCode = Reader[viewModel.BarCode].ToString(),
                            Price = Reader[viewModel.Price].ToString(),
                            Discount = Helpers.Extensions.DisplayDiscountInPercentage(Reader[viewModel.Discount].ToString()),
                            NewPrice = Reader[viewModel.NewPrice].ToString(),
                            Order = Reader[viewModel.Order].ToString()
                        });
                    }

                    Reader.Close();
                    _oleDbConnection.Close();

                    return await Task.FromResult(mapColumnViewModels);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else
            {
                return null;
            }
        }
        
        
        
        public async Task<ObservableCollection<ArticleQtycViewModel>> ReadFromExcel(ConcurrentDictionary<string, string> _myDictionary, ArticleQtycViewModel viewModel)
        {
            bool success = _myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value);
            bool sheet = _myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string sheetValue);

            if (success)
            {
                try
                {
                    string _connection =
           @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + value + ";" +
           @"Extended Properties='Excel 8.0;HDR=Yes;'";

                    _oleDbConnection = new OleDbConnection(_connection);

                    await _oleDbConnection.OpenAsync();

                    Command = new OleDbCommand();
                    Command.Connection = _oleDbConnection;
                    Command.CommandText = "select * from [" + sheetValue + "]";

                    System.Data.Common.DbDataReader Reader = await Command.ExecuteReaderAsync();

                    while (Reader.Read())
                    {

                        _articleQtycViewModels.Add(new ArticleQtycViewModel
                        {
                            Name = Reader[viewModel.Name].ToString(),
                            SubCategory = Reader[viewModel.SubCategory].ToString(),
                            Category = Reader[viewModel.Category].ToString(),
                            Storage = Reader[viewModel.Storage].ToString(),
                            BarCode = Reader[viewModel.BarCode].ToString(),
                            Price = Reader[viewModel.Price].ToString(),
                            Quantity = Reader[viewModel.Quantity].ToString()

                        });
                    }

                    Reader.Close();
                    _oleDbConnection.Close();

                    return await Task.FromResult(_articleQtycViewModels);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else
            {
                return null;
            }
        }


    }

}

