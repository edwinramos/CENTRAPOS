using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srSellOrderDetail")]
    public class DeSellOrderDetail : Base
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        public int SellOrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        public int LineNumber { get; set; }

        [Required]
        public bool IsClosed { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string ItemDescription { get; set; }
        
        public string Barcode { get; set; }

        public string ExternalCode { get; set; }

        [Required]
        public double PriceBefDiscounts { get; set; }

        public double DiscountValue { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string WarehouseCode { get; set; }

        [Required]
        public string VatCode { get; set; }

        [Required]
        public double VatPercent { get; set; }

        [Required]
        public double VatValue { get; set; }

        [Required]
        public double PriceAftVat { get; set; }

        [Required]
        public double TotalRowValue { get; set; }

        [ForeignKey("SellOrderId")]
        public virtual DeSellOrder SellOrder { get; set; }
    }
}
