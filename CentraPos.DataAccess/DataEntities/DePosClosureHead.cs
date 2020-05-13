using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srPosClosureHead")]
    public class DePosClosureHead : Base
    {
        public int PosClosureHeadId { get; set; }
        [Required]
        public string StoreCode { get; set; }
        [Required]
        public string StorePosCode { get; set; }

        public string UserCode { get; set; }

        public double BeginAmount { get; set; }

        public double Total { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
