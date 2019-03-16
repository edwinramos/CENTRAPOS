using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class ItemModel
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}