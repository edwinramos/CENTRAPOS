using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srItemWarehouse")]
    public class DeItemWarehouse : Base
    {
        [Key]
        [Column(Order = 1)]
        public string WarehouseCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string ItemCode { get; set; }

        [Required]
        public double QuantityOnHand { get; set; }

        [ForeignKey("ItemCode")]
        public virtual DeItem Item { get; set; }

        [ForeignKey("WarehouseCode")]
        public virtual DeWarehouse Warehouse { get; set; }        
    }
}
