using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srUserMobileProfile")]
    public class DeUserMobileProfile : Base
    {
        [Key]
        [Column(Order = 1)]
        public string StoreCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserCode { get; set; }

        public MobileProfileType MobileProfileType { get; set; } 

        public string Param1 { get; set; }

        public string Param2 { get; set; }

        [ForeignKey("UserCode")]
        public virtual DeUser User { get; set; }
    }

    public enum MobileProfileType
    {
        NULO = 0,
        PREVENTA = 1,
        TRANSPORTISTA = 2
    }
}
