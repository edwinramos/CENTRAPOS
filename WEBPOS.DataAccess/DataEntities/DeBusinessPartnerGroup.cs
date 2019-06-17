using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srBusinessPartnerGroup")]
    public class DeBusinessPartnerGroup : Base
    {
        [Key]
        public string BusinessPartnerGroupCode { get; set; }
        [Required]
        public string BusinessPartnerGroupDescription { get; set; }

        [ForeignKey("BusinessPartnerGroupCode")]
        public virtual IEnumerable<DeBusinessPartner> BusinessPartners { get; set; }
    }
}
