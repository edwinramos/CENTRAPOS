using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srUserMobileProfile")]
    public class DeUserMobileProfile : Base
    {
        public string StoreCode { get; set; }

        public string UserCode { get; set; }

        public MobileProfileType MobileProfileType { get; set; } 

        public string Param1 { get; set; }

        public string Param2 { get; set; }

        //[ForeignKey("UserCode")]
        public virtual DeUser User { get; set; }
    }

    public enum MobileProfileType
    {
        NULO = 0,
        PREVENTA = 1,
        TRANSPORTISTA = 2
    }
}
