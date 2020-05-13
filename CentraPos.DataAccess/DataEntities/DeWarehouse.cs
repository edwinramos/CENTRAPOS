using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srWarehouse")]
    public class DeWarehouse : Base
    {
        public string WarehouseCode { get; set; }
        public string WarehouseDescription { get; set; }
    }
}
