using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.Models
{
    public class SellTransactionHeadModel
    {
        public DateTime TransactionDateTime { get; set; }

        public string NCF { get; set; }

        public string StorePosCode { get; set; }

        public string Customer { get; set; }

        public double TransactionNumber { get; set; }

        public double TotalValue { get; set; }
    }
}