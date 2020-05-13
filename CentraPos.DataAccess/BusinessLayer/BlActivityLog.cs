using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlActivityLog
    {
        public static IEnumerable<DeActivityLog> ReadAll()
        {
            var dl = new DlActivityLog();
            return dl.ReadAll();
        }
        public static IQueryable<DeActivityLog> ReadAllQueryable()
        {
            var dl = new DlActivityLog();
            return dl.ReadAllQueryable();
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
