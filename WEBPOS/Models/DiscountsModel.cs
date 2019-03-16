using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Models
{
    public class DiscountsModel
    {
        public string ItemCode { get; set; }
        public double ItemPrice { get; set; }
        public double DiscountAmount { get; set; }
        public double Result { get; set; }
        public DiscountType DiscountType { get; set; }
    }

    public enum DiscountType
    {
        Porcentual = 0,
        Monto = 1
    }
}