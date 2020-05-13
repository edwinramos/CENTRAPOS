using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srPaymentType")]
    public class DePaymentType : Base
    {
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeDescription { get; set; }

        //public virtual ICollection<DeItem> Items { get; set; }
    }
}
