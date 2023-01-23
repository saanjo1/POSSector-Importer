using ImportApp.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ImportApp.WPF.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlServer(context.Configuration.GetConnectionString("sqlstring"));

                services.AddDbContext<ImportAppDbContext>(configureDbContext);
                services.AddSingleton<ImportAppDbContextFactory>(new ImportAppDbContextFactory(configureDbContext));
            });

            return hostBuilder;
        }
    }
}
