using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.Models
{
    public class BusinessPartnerModel
    {
        public string BusinessPartnerCode { get; set; }
        public string BusinessPartnerDescription { get; set; }
        public string PriceListCode { get; set; }
        public string PriceListDescription { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}