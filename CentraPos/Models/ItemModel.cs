using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentraPos.Models
{
    public class ItemModel
    {
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string TaxCode { get; set; }

        public string SupplierCode { get; set; }

        public double NetWeight { get; set; }
        public string UnitMeasureCode { get; set; }
        public string DepartmentCode { get; set; }
        public string Barcode { get; set; }
        public bool IsVoided { get; set; }
        public bool canEdit { get; set; }
    }
}