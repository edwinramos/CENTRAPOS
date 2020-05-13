using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srPosClosureDetail")]
    public class DePosClosureDetail : Base
    {
        public int PosClosureHeadId { get; set; }

        public string StoreCode { get; set; }

        public string StorePosCode { get; set; }

        public double TransactionNumber { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string NCF { get; set; }

        public double TotalValue { get; set; }
    }
}
