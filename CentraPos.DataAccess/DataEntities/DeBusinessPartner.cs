using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srBusinessPartner")]
    public class DeBusinessPartner : Base
    {
        public string BusinessPartnerCode { get; set; }
        public string BusinessPartnerType { get; set; }
        public string BusinessPartnerDescription { get; set; }
        public string RNC { get; set; }
        public string PriceListCode { get; set; }
        public string BusinessPartnerGroupCode { get; set; }
        public bool IsVoided { get; set; }
        
        public virtual DePriceList PriceList { get; set; }

        ////[ForeignKey("BusinessPartnerCode")]
        //public virtual IEnumerable<DeBusinessPartnerContact> BusinessPartnerContacts { get; set; }

        public virtual DeBusinessPartnerGroup BusinessPartnerGroup { get; set; }
    }
}
