using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class PriceModel
    {
        public string PriceListCode { get; set; }
        public string PriceList { get; set; }
        public string ItemCode { get; set; }
        public double SellPrice { get; set; }
    }
}