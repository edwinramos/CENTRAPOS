using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Models
{
    public class PaymentModel
    {
        public string PaymentTypeCode { get; set; }
        public double Total { get; set; }
        public double PayedAmount { get; set; }
        public double Rest { get; set; }
        public string PosCode { get; set; }
        public string CustomerCode { get; set; }
        public string PriceListCode { get; set; }
        public string StoreCode { get; set; }
        public DocType DocType { get; set; }
    }
}