using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srSellTransactionPayment")]
    public class DeSellTransactionPayment : Base
    {
        public string StoreCode { get; set; }

        public string PosCode { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public double TransactionNumber { get; set; }

        public long RowNumber { get; set; }

        public string PaymentTypeCode { get; set; }

        [Required]
        public double PaymentValue { get; set; }

        #region Foreign Keys
        //[ForeignKey("StoreCode,PosCode,TransactionDateTime,TransactionNumber")]
        public virtual DeSellTransactionHead SellTransactionHead { get; set; }
        #endregion
    }
}