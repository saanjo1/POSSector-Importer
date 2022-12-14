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

namespace ImportApp.WPF.Services
{
    public class ExcelDataService : IExcelDataService
    {

        public static string ExcelFile { get; set; }

        public static OleDbConnection _oleDbConnection;
        public static OleDbCommand Command;

        private static ObservableCollection<MapColumnViewModel> mapColumnViewModels = new ObservableCollection<MapColumnViewModel>();

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


        public async Task<List<string>> ListSheetsFromFile()
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = ExcelFile;
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
                while(fieldCount >= fieldIncrementor)
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

        public async Task<ObservableCollection<MapColumnViewModel>> ReadFromExcel(MapColumnViewModel mColumnModel)
        {
            try
            {
                string _connection =
       @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFile + ";" +
       @"Extended Properties='Excel 8.0;HDR=Yes;'";

                _oleDbConnection = new OleDbConnection(_connection);

                await _oleDbConnection.OpenAsync();

                Command = new OleDbCommand();
                Command.Connection = _oleDbConnection;
                Command.CommandText = "select * from [" + App.Current.Properties["SheetName"] + "]";

                var Reader = await Command.ExecuteReaderAsync();

                while (Reader.Read())
                {
                    mapColumnViewModels.Add(new MapColumnViewModel()
                    {
                        Name = Reader[mColumnModel.Name].ToString(),
                        ArticleNumber = Reader[mColumnModel.ArticleNumber].ToString(),
                        BarCode = Reader[mColumnModel.BarCode].ToString(),
                        Price = Reader[mColumnModel.Price].ToString(),
                        Gender = Reader[mColumnModel.Gender].ToString(),
                        Collection = Reader[mColumnModel.Collection].ToString(),
                        Quantity = Reader[mColumnModel.Quantity].ToString(),
                        Order = Reader[mColumnModel.Order].ToString(),
                    });
                }

                Reader.Close();
                _oleDbConnection.Close();

                return mapColumnViewModels;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
