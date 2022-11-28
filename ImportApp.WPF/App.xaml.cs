using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.DBContext;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.State.Navigators;
using ImportApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
              .ConfigureServices((context, services) =>
              {
                  services.AddSingleton<ImportAppDbContextFactory>();

                  //services.AddSingleton<MainViewModel>();
                  //services.AddSingleton<IArticleService>();

                  services.AddSingleton((x) => new MainWindow
                  {
                  });

              }).Build();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host!.StartAsync();

            var contextFactory = _host.Services.GetRequiredService<ImportAppDbContextFactory>();

            using(ImportAppDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.EnsureCreated();
            }

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

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
