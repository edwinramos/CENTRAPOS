using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srPaymentType")]
    public class DePaymentType : Base
    {
        [Key]
        public string PaymentTypeCode { get; set; }
        [Required]
        public string PaymentTypeDescription { get; set; }

        //public virtual ICollection<DeItem> Items { get; set; }
    }
}
