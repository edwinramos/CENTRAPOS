using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.Models
{
    public class StorePosModel
    {
        public string StoreCode { get; set; }
        public string StoreDescription { get; set; }
        public string StorePosCode { get; set; }
        public string StorePosDescription { get; set; }
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
    }
}