﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srItem")]
    public class DeItem : Base
    {
        [Key]
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

        [ForeignKey("TaxCode")]
        public virtual DeTax Tax { get; set; }
        [ForeignKey("DepartmentCode")]
        public virtual DeDepartment Department { get; set; }
        [ForeignKey("UnitMeasureCode")]
        public virtual DeUnitMeasure UnitMeasure { get; set; }

        //[ForeignKey("SupplierCode")]
        //public virtual DeBusinessPartner BusinessPartner { get; set; }

        //[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DePrice> Prices { get; set; }

        //[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DeItemWarehouse> ItemWarehouses { get; set; }

        //[ForeignKey("ItemCode")]
        //public virtual IEnumerable<DeSellTransactionDetail> SellTransactionDetails { get; set; }
    }
}