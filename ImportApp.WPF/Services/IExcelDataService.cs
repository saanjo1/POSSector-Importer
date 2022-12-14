using ImportApp.WPF.Services;
using ImportApp.WPF.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IExcelDataService : IExcelServiceProvider<MapColumnViewModel>
    {
        
    }
}
