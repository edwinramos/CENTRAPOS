using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srTax")]
    public class DeTax : Base
    {
        public string TaxCode { get; set; }
        [Required]
        public string TaxDescription { get; set; }
        [Required]
        public double TaxPercent { get; set; }

        ////[ForeignKey("TaxCode")]
        //public virtual ICollection<DeItem> Items { get; set; }
    }
}
