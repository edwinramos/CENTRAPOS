using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBPOS.Models
{
    public class PosClosureModel
    {
        public int PosClosureHeadId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string UserCode { get; set; }

        public string Estado { get; set; }

        public double BeginAmount { get; set; }

        public double Total { get; set; }
    }
}