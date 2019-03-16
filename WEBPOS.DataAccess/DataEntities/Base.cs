using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS.DataAccess.DataEntities
{
    public class Base
    {
        public DateTime LastUpdate { get; set; }
        public string UpdateUser { get; set; }
    }
}
