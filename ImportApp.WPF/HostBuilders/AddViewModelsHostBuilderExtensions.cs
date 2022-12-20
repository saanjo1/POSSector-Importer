using ImportApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host) 
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<StoreViewModel>();
                services.AddTransient<MapColumnViewModel>();
                services.AddTransient<MapDataViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<ImportDataViewModel>();
                services.AddTransient<HomeViewModel>();
            });

            return host;
            
        }

    }
}
