using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class SellOrderDetailModel
    {
        public int SellOrderId { get; set; }

        public int LineNumber { get; set; }

        public bool IsClosed { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string Barcode { get; set; }

        public string ExternalCode { get; set; }

        public double PriceBefDiscounts { get; set; }

        public double DiscountValue { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public string WarehouseCode { get; set; }

        public string VatCode { get; set; }

        public double VatPercent { get; set; }

        public double VatValue { get; set; }

        public double PriceAftVat { get; set; }

        public double TotalRowValue { get; set; }
    }
}