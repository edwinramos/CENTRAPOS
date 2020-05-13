using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srItem")]
    public class DeItem : Base
    {
        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        //[Required]
        public string TaxCode { get; set; }
        //[Required]
        public string SupplierCode { get; set; }

        public double NetWeight { get; set; }
        public string UnitMeasureCode { get; set; }
        public string DepartmentCode { get; set; }
        public string Barcode { get; set; }
        public bool IsVoided { get; set; }
        public ShortageLevels ShortageLevel { get; set; }

        
        public virtual DeTax Tax { get; set; }
        
        public virtual DeDepartment Department { get; set; }
        
        public virtual DeUnitMeasure UnitMeasure { get; set; }

        ////[ForeignKey("SupplierCode")]
        //public virtual DeBusinessPartner BusinessPartner { get; set; }

        ////[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DePrice> Prices { get; set; }

        ////[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DeItemWarehouse> ItemWarehouses { get; set; }

        ////[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DeSellTransactionDetail> SellTransactionDetails { get; set; }
    }

    public enum ShortageLevels
    {
        NONE = 0,
        ALERT = 1,
        RED = 2,
        YELLOW = 3,
        GREEN = 4
    }
}
