using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srDepartment")]
    public class DeDepartment : Base
    {
        [Key]
        public string DepartmentCode { get; set; }
        public string DepartmentDescription { get; set; }

        //[ForeignKey("DepartmentCode")]
        //public virtual ICollection<DeItem> Items { get; set; }
    }
}
