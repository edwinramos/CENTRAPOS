using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srSellTransactionPayment")]
    public class DeSellTransactionPayment : Base
    {
        [Required]
        [Key]
        [Column(Order = 0)]
        public string StoreCode { get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public string PosCode { get; set; }

        [Required]
        [Key]
        [Column(Order = 2)]
        public DateTime TransactionDateTime { get; set; }

        [Required]
        [Key]
        [Column(Order = 3)]
        public double TransactionNumber { get; set; }

        [Required]
        [Key]
        [Column(Order = 4)]
        public long RowNumber { get; set; }

        public string PaymentTypeCode { get; set; }

        [Required]
        public double PaymentValue { get; set; }

        #region Foreign Keys
        [ForeignKey("StoreCode,PosCode,TransactionDateTime,TransactionNumber")]
        public virtual DeSellTransactionHead SellTransactionHead { get; set; }
        #endregion
    }
}