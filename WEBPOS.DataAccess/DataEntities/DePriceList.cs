using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srPriceList")]
    public class DePriceList : Base
    {
        [Key]
        public string PriceListCode { get; set; }
        public string PriceListDescription { get; set; }

        [ForeignKey("PriceListCode")]
        public virtual ICollection<DePrice> Prices { get; set; }
    }
}
