using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srWarehouse")]
    public class DeWarehouse : Base
    {
        [Key]
        public string WarehouseCode { get; set; }
        public string WarehouseDescription { get; set; }
    }
}
