using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlActivityLog
    {
        public static IEnumerable<DeActivityLog> ReadAll()
        {
            var dl = new DlActivityLog();
            return dl.ReadAll();
        }
        public static IEnumerable<DeActivityLog> ReadAllQueryable()
        {
            var dl = new DlActivityLog();
            return dl.ReadAll();
        }
        
        public static void Save(DeActivityLog obj)
        {
            var dl = new DlActivityLog();
            dl.Save(obj);
        }

        public static void Delete(DeActivityLog obj)
        {
            var dl = new DlActivityLog();
            dl.Delete(obj.ID);
        }
    }
}
