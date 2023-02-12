using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.Services;
using ImportApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImportApp.WPF.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IGenericBaseInterface<Article>, ArticleService>();
                services.AddSingleton<IArticleService, ArticleService>();

                services.AddSingleton<IExcelServiceProvider<MapColumnForDiscountViewModel>, ExcelDataService>();
                services.AddSingleton<IExcelDataService, ExcelDataService>();

                services.AddSingleton<IGenericBaseInterface<Rule>, RuleService>();
                services.AddSingleton<IRuleService, RuleService>();

                services.AddSingleton<IGenericBaseInterface<Category>, CategoryService>();
                services.AddSingleton<ICategoryService, CategoryService>();

                services.AddSingleton<IGenericBaseInterface<Storage>, StorageService>();
                services.AddSingleton<IStorageService, StorageService>();

                services.AddSingleton<IGenericBaseInterface<Supplier>, SupplierService>();
                services.AddSingleton<ISupplierService, SupplierService>();
            });

            return host;
        }
    }
}
