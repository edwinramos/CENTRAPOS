using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srPrice")]
    public class DePrice : Base
    {
        public string PriceListCode { get; set; }
        public string ItemCode { get; set; }

        [Required]
        public double SellPrice { get; set; }

        public virtual DeItem Item { get; set; }
        public virtual DePriceList PriceList { get; set; }
    }
}
