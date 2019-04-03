using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class ImportedItem
    {
        public string Producto { get; set; }
        public string Cantidad { get; set; }
        
        public string PrecioList { get; set; }
        
        public string Barcode { get; set; }
        public string TaxPercent { get; set; }
    }
}