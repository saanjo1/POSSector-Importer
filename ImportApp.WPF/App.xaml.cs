using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImportApp.WPF.HostBuilders;
using System;
using System.Windows;
using System.IO;
using Microsoft.Extensions.Configuration;
using ImportApp.Domain.Models;

namespace ImportApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static IHost _host { get; private set; }


        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddDbContext()
                .AddViewModels()
                .AddServices()
                .AddConfiguration()
                .AddViews().Build();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host!.StartAsync();

            var contextFactory = _host.Services.GetRequiredService<ImportAppDbContextFactory>();

            using(ImportAppDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.EnsureCreated();
            }

            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }


        protected override async void OnExit(ExitEventArgs e)
        {
           await _host!.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
