using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srSellTransactionDetail")]
    public class DeSellTransactionDetail : Base
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

        [Key]
        [Required]
        [Column(Order = 5)]
        public double RowNumber { get; set; }

        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Barcode { get; set; }
        public string TaxCode { get; set; }
        public double TaxPercent { get; set; }

        public double BasePrice { get; set; }
        public double SellPrice { get; set; }
        public double DiscountOnItem { get; set; }
        public double Quantity { get; set; }
        public double RowValue { get; set; }

        public string PriceListCode { get; set; }

        public double TotalValue { get; set; }

        public bool IsPrinted { get; set; }

        public virtual DeItem Item { get; set; }
        public virtual DePriceList PriceList { get; set; }
        public virtual DeStore Store { get; set; }
        public virtual DeStorePos StorePos { get; set; }
    }
}
