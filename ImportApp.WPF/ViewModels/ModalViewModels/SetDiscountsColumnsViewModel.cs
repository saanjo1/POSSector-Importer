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
    public partial class SetDiscountsColumnsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string category;

        [ObservableProperty]
        private string barcode;

        [ObservableProperty]
        private string fullPrice;

        [ObservableProperty]
        private string discount;

        [ObservableProperty]
        private string itemsize;

        [ObservableProperty]
        private string item;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string discountedPrice;

        private SettingsViewModel settingsViewModel;
        private Notifier _notifier;


        public SetDiscountsColumnsViewModel(SettingsViewModel viewModel, Notifier notifier)
        {
            LoadData();
            settingsViewModel = viewModel;
            _notifier = notifier;
        }

        private void LoadData()
        {
            var json = File.ReadAllText("appconfigsettings.json");
            var settings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            Name = settings["Discounts"]["Name"];
            Category = settings["Discounts"]["Category"];
            Barcode = settings["Discounts"]["BarCode"];
            FullPrice = settings["Discounts"]["FullPrice"];
            Discount = settings["Discounts"]["Discount"];
            DiscountedPrice = settings["Discounts"]["DiscountedPrice"];
            Itemsize = settings["Discounts"]["ItemSize"];
            Item = settings["Discounts"]["Item"];
            Description = settings["Discounts"]["Description"];
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
                    jsonObject["Discounts"]["Name"] = Name;
                    jsonObject["Discounts"]["Category"] = Category;
                    jsonObject["Discounts"]["Barcode"] = Barcode;
                    jsonObject["Discounts"]["FullPrice"] = FullPrice;
                    jsonObject["Discounts"]["Discount"] = Discount;
                    jsonObject["Discounts"]["DiscountedPrice"] = DiscountedPrice;
                    jsonObject["Discounts"]["ItemSize"] = Itemsize;
                    jsonObject["Discounts"]["Item"] = Item;
                    jsonObject["Discounts"]["Description"] = Description;

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
