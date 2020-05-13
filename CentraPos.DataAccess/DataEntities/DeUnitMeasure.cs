using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srUnitMeasure")]
    public class DeUnitMeasure : Base
    {
        public string UnitMeasureCode { get; set; }
        public string UnitMeasureDescription { get; set; }

        //[ForeignKey("UnitMeasureCode")]
        public virtual ICollection<DeItem> Items { get; set; }
    }
}
