using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srStore")]
    public class DeStore : Base
    {
        public string StoreCode { get; set; }
        [Required]
        public string StoreDescription { get; set; }
        [Required]
        public string PriceListCode { get; set; }
        [Required]
        public string WarehouseCode { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string RNC { get; set; }

        public string NIF { get; set; }

        public int NCFSequence1 { get; set; }

        public int NCFSequence2 { get; set; }

        public double MaxDiscAmount { get; set; }

        public double MaxDiscPercent { get; set; }

        public DateTime SequenceDueDate { get; set; }

        //[ForeignKey("PriceListCode")]
        public virtual DePriceList PriceList { get; set; }
        //[ForeignKey("WarehouseCode")]
        public virtual DeWarehouse Warehouse { get; set; }
        //[ForeignKey("StoreCode")]
        public virtual IEnumerable<DeStorePos> StorePoses { get; set; }
    }
}
