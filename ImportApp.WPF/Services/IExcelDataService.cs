using ImportApp.Domain.Services;
using ImportApp.WPF;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.Services
{
    public class IExcelDataService : IExcelService
    {
        public async Task<string> OpenDialog()
        {
            OpenFileDialog openFIleDialog = WPF.Helpers.Extensions.CreateOFDialog();

            if (openFIleDialog.ShowDialog() == true)
            {
                string? excelFile = openFIleDialog.FileName;
                return await Task.FromResult(excelFile);
            }

            return null;
        }
    }
}
