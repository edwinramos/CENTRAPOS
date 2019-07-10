using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srBusinessPartner")]
    public class DeBusinessPartner : Base
    {
        [Key]
        [Column(Order = 1)]
        public string BusinessPartnerCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string BusinessPartnerType { get; set; }
        public string BusinessPartnerDescription { get; set; }
        public string RNC { get; set; }
        public string PriceListCode { get; set; }
        public string BusinessPartnerGroupCode { get; set; }
        public bool IsVoided { get; set; }
        [ForeignKey("PriceListCode")]
        public virtual DePriceList PriceList { get; set; }

        //[ForeignKey("BusinessPartnerCode")]
        //public virtual IEnumerable<DeBusinessPartnerContact> BusinessPartnerContacts { get; set; }

        [ForeignKey("BusinessPartnerGroupCode")]
        public virtual DeBusinessPartnerGroup BusinessPartnerGroup { get; set; }
    }
}
