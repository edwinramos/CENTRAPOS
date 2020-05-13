using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srStorePos")]
    public class DeStorePos : Base
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public string StoreCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        public string StorePosCode { get; set; }

        [Required]
        public string StorePosDescription { get; set; }

        public string DeviceId { get; set; }

        public DeviceType DeviceType { get; set; }

        
        public virtual DeStore Store { get; set; }
    }
    public enum DeviceType
    {
        DESKTOP = 0,
        MOBILE = 1
    }
}
