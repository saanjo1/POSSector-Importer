using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.Services;
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
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IDataGService<Article>, ArticleDataService>();
                services.AddSingleton<IArticleDataService, ArticleDataService>();

                services.AddSingleton<IExcelServiceProvider<MapColumnViewModel>, ExcelDataService>();
                services.AddSingleton<IExcelDataService, ExcelDataService>();

                services.AddSingleton<IDataGService<Rule>, DiscountDataService>();
                services.AddSingleton<IDiscountDataService, DiscountDataService>();

                services.AddSingleton<IDataGService<Category>, CategoryDataService>();
                services.AddSingleton<ICategoryDataService, CategoryDataService>();
            });

            return host;
        }
    }
}
