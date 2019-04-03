using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Models
{
    public class PosModel
    {
        public string StoreCode { get; set; }
        public string StoreDescription { get; set; }
        public string StorePosCode { get; set; }
        public string StorePosDescription { get; set; }
        public double MaxDiscAmount { get; set; }
        public double MaxDiscPercent { get; set; }
        public int PosClosureId { get; set; }
        public bool IsOpenPos { get; set; }
    }
}