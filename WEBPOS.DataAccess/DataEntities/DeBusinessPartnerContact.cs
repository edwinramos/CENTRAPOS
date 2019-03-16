using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srBusinessPartnerContact")]
    public class DeBusinessPartnerContact : Base
    {
        [Key]
        [Column(Order = 1)]
        public string BusinessPartnerCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string BusinessPartnerContactCode { get; set; }

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Email { get; set; }

        //[ForeignKey("BusinessPartnerCode")]
        //public virtual DeBusinessPartner BusinessPartner { get; set; }
    }
}
