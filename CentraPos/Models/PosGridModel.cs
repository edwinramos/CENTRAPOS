using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.Models
{
    public class PosGridModel
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Barcode { get; set; }
        public string PriceListCode { get; set; }
        public string PriceListDescription { get; set; }
        public double Quantity { get; set; }
        public double SellPrice { get; set; }
        public double Discount { get; set; }
        public string TaxDescription { get; set; }
        public string TaxCode { get; set; }
        public double TaxPercent { get; set; }
        public double VatPrice { get; set; }
        public double Total { get; set; }
        public int DiscountType { get; set; }
    }
}