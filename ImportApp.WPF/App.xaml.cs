using ImportApp.EntityFramework.DBContext;
using ImportApp.WPF.Helpers;
using ImportApp.WPF.HostBuilders;
using ImportApp.WPF.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Xml;

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

            using (ImportAppDbContext context = contextFactory.CreateDbContext())
            {
                await context.Database.EnsureCreatedAsync();
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
