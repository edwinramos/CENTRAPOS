using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srUserSellOrder")]
    public class DeUserSellOrder : Base
    {
        [Key]
        [Column(Order = 1)]
        public string UserCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public int SellOrderId { get; set; }

        public UserOrderState UserOrderState { get; set; }

        //[ForeignKey("PriceListCode")]
        //public virtual ICollection<DePrice> Prices { get; set; }
    }

    public enum UserOrderState
    {
        ASIGNADA = 0,
        TRABAJANDO = 1,
        CONCLUIDO = 2
    }
}
