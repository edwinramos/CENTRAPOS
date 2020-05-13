using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srSellTransactionHead")]
    public class DeSellTransactionHead : Base
    {
        [Key]
        [Column(Order = 1)]
        public string StoreCode { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public string PosCode { get; set; }

        [Key]
        [Required]
        [Column(Order = 3)]
        public DateTime TransactionDateTime { get; set; }

        [Key]
        [Required]
        [Column(Order = 4)]
        public double TransactionNumber { get; set; }

        public string CustomerCode { get; set; }

        public string PriceListCode { get; set; }

        public int SellOrderId { get; set; }

        public string NCF { get; set; }

        public double TotalValue { get; set; }
        public double TotalDiscount { get; set; }
        public bool IsPrinted { get; set; }

        public DocType DocType { get; set; }
        public virtual DeStore Store { get; set; }
    }

    public enum DocType
    {
        CreditoFiscal = 0,
        ConsumidorFinal = 1,
        Gubernamental = 2
    }
}
