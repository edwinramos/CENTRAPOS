using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentraPos.DataAccess.Models
{
    public class ItemSearchResult
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double AvailableQty { get; set; }
        public double Price { get; set; }
    }
}
