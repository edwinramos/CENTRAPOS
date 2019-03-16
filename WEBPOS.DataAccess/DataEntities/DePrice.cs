using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srPrice")]
    public class DePrice : Base
    {
        [Key]
        [Column(Order = 1)]
        public string PriceListCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string ItemCode { get; set; }

        [Required]
        public double SellPrice { get; set; }

        public virtual DeItem Item { get; set; }
        public virtual DePriceList PriceList { get; set; }
    }
}
