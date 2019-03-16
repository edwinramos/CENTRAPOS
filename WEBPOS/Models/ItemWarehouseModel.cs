using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class ItemWarehouseModel
    {
        public string WarehouseCode { get; set; }
        public string Warehouse { get; set; }
        public string ItemCode { get; set; }
        public double QuantityOnHand { get; set; }
    }
}