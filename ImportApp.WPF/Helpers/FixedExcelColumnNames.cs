using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImportApp.WPF.Helpers
{
    public class FixedExcelColumnNames
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Storage { get; set; }
        public string BarCode { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string PricePerUnit { get; set; }
        public string Item { get; set; }
        public string ColorDescription { get; set; }
        public string ItemSize { get; set; }




        public FixedExcelColumnNames()
        {
            var json = File.ReadAllText("appconfigsettings.json");
            var settings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            Name = settings["Articles"]["Name"];
            Category = settings["Articles"]["Category"];
            Storage = settings["Articles"]["Storage"];
            BarCode = settings["Articles"]["BarCode"];
            Price = settings["Articles"]["Price"];
            Quantity = settings["Articles"]["Quantity"];
            PricePerUnit = settings["Articles"]["PricePerUnit"];
            ItemSize = settings["Articles"]["ItemSize"];
            Item = settings["Articles"]["Item"];
            ColorDescription = settings["Articles"]["ColorDescription"];
        }

    }


    public class FixedDiscountColumnNames
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string BarCode { get; set; }
        public string Discount { get; set; }
        public string FullPrice { get; set; }
        public string DiscountedPrice { get; set; }
        public string Description { get; set; }
        public string Item { get; set; }
        public string ColorDescription { get; set; }
        public string ItemSize { get; set; }





        public FixedDiscountColumnNames()
        {
            var json = File.ReadAllText("appconfigsettings.json");
            var settings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            Name = settings["Discounts"]["Name"];
            Category = settings["Discounts"]["Category"];
            BarCode = settings["Discounts"]["BarCode"];
            FullPrice = settings["Discounts"]["FullPrice"];
            Description = settings["Discounts"]["Description"];
            Discount = settings["Discounts"]["Discount"];
            ItemSize = settings["Discounts"]["ItemSize"];
            Item = settings["Discounts"]["Item"];
            DiscountedPrice = settings["Discounts"]["DiscountedPrice"];
        }

    }


    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string sqlstring { get; set; }
    }
}
