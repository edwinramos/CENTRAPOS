using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srBusinessPartnerContact")]
    public class DeBusinessPartnerContact : Base
    {
        public string BusinessPartnerCode { get; set; }

        public string BusinessPartnerContactCode { get; set; }

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Email { get; set; }

        ////[ForeignKey("BusinessPartnerCode")]
        //public virtual DeBusinessPartner BusinessPartner { get; set; }
    }
}
