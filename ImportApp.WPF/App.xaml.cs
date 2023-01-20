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
using ImportApp.WPF.Resources;
using System.Xml;
using ImportApp.WPF.Helpers;
using Newtonsoft.Json;

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

            UpdateConnectionString();


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


        private void UpdateConnectionString()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int index = appDataPath.IndexOf("Roaming");
            appDataPath = appDataPath.Substring(0, index) + Translations.POSFolderPath;

            XmlDocument doc = new XmlDocument();
            doc.Load(appDataPath);

            XmlNode dataSource = doc.SelectSingleNode(Translations.DataSourcePath);



            string json = File.ReadAllText("appsettings.json");
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(json);
            settings.ConnectionStrings.sqlstring = dataSource.InnerText + Translations.Encrypt;

            json = JsonConvert.SerializeObject(settings);
            File.WriteAllText("appsettings.json", json);
        }
    }
}
