using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.Models
{
    public class SecurityModel
    {
        public string StoreCode { get; set; }
        public double MaxDiscPercent { get; set; }
        public double MaxDiscAmount { get; set; }
        public string EnableEditing { get; set; }
    }
}