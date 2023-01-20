using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Name = "NAME";
            Category = "GENDER";
            Storage = "STORAGE";
            BarCode = "BARCODE";
            Price = "SO_PRICE";
            Quantity = "QTYC";
            PricePerUnit = "PRICEUNIT";
            ItemSize = "ITEM_SIZE";
            Item = "ITEM";
            ColorDescription = "COLOR_DESCRIPTION";
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
            Name = "NAME";
            Category = "Department (EN)";
            BarCode = "Barcode";
            Discount = "Discount";
            FullPrice = "Full price €";
            DiscountedPrice = "Discounted price €";
            Description = "Description (EN)";
            ItemSize = "Size";
            Item = "Product";
            ColorDescription = "Color_Description (EN)";
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
