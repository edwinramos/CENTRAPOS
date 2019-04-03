using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srPosClosureDetail")]
    public class DePosClosureDetail : Base
    {
        [Key]
        [Column(Order = 1)]
        public int PosClosureHeadId { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public string StoreCode { get; set; }

        [Key]
        [Required]
        [Column(Order = 3)]
        public string StorePosCode { get; set; }

        [Key]
        [Required]
        [Column(Order = 4)]
        public double TransactionNumber { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string NCF { get; set; }

        public double TotalValue { get; set; }
    }
}
