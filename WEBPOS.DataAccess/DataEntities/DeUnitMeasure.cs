using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srUnitMeasure")]
    public class DeUnitMeasure : Base
    {
        [Key]
        public string UnitMeasureCode { get; set; }
        public string UnitMeasureDescription { get; set; }

        [ForeignKey("UnitMeasureCode")]
        public virtual ICollection<DeItem> Items { get; set; }
    }
}
