using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentraPos.DataAccess.Models
{
    public class SoldQuantityModel
    {
        public string UserCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ItemDescription { get; set; }
        public double Quantity { get; set; }
    }
}
