using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlPosClosureDetail
    {
        public static IEnumerable<DePosClosureDetail> ReadAll()
        {
            var dl = new DlPosClosureDetail();
            return dl.ReadAll();
        }
        public static IQueryable<DePosClosureDetail> ReadAllQueryable()
        {
            var dl = new DlPosClosureDetail();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DePosClosureDetail> Read(DePosClosureDetail obj)
        {
            var dl = new DlPosClosureDetail();
            return dl.Read(obj);
        }

        public static void Save(DePosClosureDetail obj)
        {
            var dl = new DlPosClosureDetail();
            dl.Save(obj);
        }
    }
}
