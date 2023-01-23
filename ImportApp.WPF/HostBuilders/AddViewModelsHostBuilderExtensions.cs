using ImportApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImportApp.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<StoreViewModel>();
                services.AddTransient<MapColumnForDiscountViewModel>();
                services.AddTransient<SelectExcelSheetModalViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<ImportDataViewModel>();
                services.AddTransient<HomeViewModel>();
            });

            return host;

        }

    }
}
