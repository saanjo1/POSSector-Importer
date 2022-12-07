using System.Collections.ObjectModel;

namespace ImportApp.Domain.Services
{
    public interface IExcelService 
    {
        Task<string> OpenDialog();
        Task<List<string>> ListSheetsFromFile();
        Task<List<string>> ListColumnNames(string excelName);
        
    }
}
