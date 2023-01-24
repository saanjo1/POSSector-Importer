using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.WPF.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    public partial class SetArticlesColumnsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string category;

        [ObservableProperty]
        private string storage;

        [ObservableProperty]
        private string barcode;

        [ObservableProperty]
        private string price;

        [ObservableProperty]
        private string quantity;

        [ObservableProperty]
        private string itemsize;

        [ObservableProperty]
        private string item;

        [ObservableProperty]
        private string priceperunit;

        [ObservableProperty]
        private string colordescription;

        private SettingsViewModel settingsViewModel;
        private Notifier _notifier;


        public SetArticlesColumnsViewModel(SettingsViewModel viewModel, Notifier notifier)
        {
            settingsViewModel = viewModel;
            _notifier = notifier;
            LoadData();
        }

        private void LoadData()
        {
            var json = File.ReadAllText("appconfigsettings.json");
            var settings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            Name = settings["Articles"]["Name"];
            Category = settings["Articles"]["Category"];
            Storage = settings["Articles"]["Storage"];
            Barcode = settings["Articles"]["BarCode"];
            Price = settings["Articles"]["Price"];
            Quantity = settings["Articles"]["Quantity"];
            Priceperunit = settings["Articles"]["PricePerUnit"];
            Itemsize = settings["Articles"]["ItemSize"];
            Item = settings["Articles"]["Item"];
            Colordescription = settings["Articles"]["ColorDescription"];
        }

        [RelayCommand]
        public void Cancel()
        {
            settingsViewModel.Cancel();
        }

        [RelayCommand]
        public void Save()
        {
            try
            {
                using (FileStream fs = new FileStream("appconfigsettings.json", FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    //Read the file into memory
                    byte[] jsonBytes = new byte[fs.Length];
                    fs.Read(jsonBytes, 0, (int)fs.Length);

                    //Deserialize the json string to dynamic object
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(jsonBytes));

                    //Modify the json object
                    jsonObject["Articles"]["Name"] = Name;
                    jsonObject["Articles"]["Category"] = Category;
                    jsonObject["Articles"]["Storage"] = Storage;
                    jsonObject["Articles"]["Barcode"] = Barcode;
                    jsonObject["Articles"]["Price"] = Price;
                    jsonObject["Articles"]["Quantity"] = Quantity;
                    jsonObject["Articles"]["PricePerUnit"] = Priceperunit;
                    jsonObject["Articles"]["ItemSize"] = Itemsize;
                    jsonObject["Articles"]["Item"] = Item;
                    jsonObject["Articles"]["ColorDescription"] = Colordescription;

                    //Serialize the json object back to string
                    string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                    //Set the position to the beginning of the file
                    fs.Position = 0;

                    //Write the new json string back to the file
                    byte[] jsonBytesNew = Encoding.UTF8.GetBytes(json);
                    fs.Write(jsonBytesNew, 0, jsonBytesNew.Length);
                    settingsViewModel.Cancel();
                    _notifier.ShowSuccess("Successfully saved column names.");
                }
            }
            catch (Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);
                settingsViewModel.Cancel();
                throw;
            }
        }

    }
}
